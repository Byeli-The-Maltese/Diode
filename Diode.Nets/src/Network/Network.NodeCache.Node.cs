namespace Diode.Nets;

partial class Network
{
    partial class NodeCache
    {
        internal abstract class Node(SpiceName name, Network network, ulong id, Sub scope) : CacheItem(network, id, scope)
        {
            public SpiceName Name { get; } = name;

            public L6 State { get; protected private set; } = L6.U;

            public LinkId PusherSrc { get; protected set; } = default; // Default means HiZ is what's pushed

            public ulong PushCount { get; protected set; } = 0;

            protected sealed override Cache<ulong, Node> GetMyContainer() => network.nodes;

            internal string GetFullName()
                => network
                .subs
                .TryGet(Scope)
                .Map(s => s.NameSpace.Prefix + SpiceName.NetSep + Name)
                .OrElse(Name);
        }

        internal sealed class Node<T> : Node
        where T : IEquatable<T>
        {
            // // // fields
            private readonly Dictionary<LinkId, Voltage<T>> inputs = new(capacity: 2);
            private HashSet<LinkId>? linkDrives = null;
            private bool alreadyDisposed = false;

            // // // constructor

            public Node(SpiceName Name, Network network, Net<T> id, Sub scope) : base(Name, network, id, scope)
            {
                this.network.LogNodeCreation(this);
            }

            // // // methods

            internal Voltage<T> GetImmediateSample()
                => PusherSrc == default
                ? default
                : inputs[PusherSrc];

            private void LookupAndDriveLink(LinkId linkId, Voltage<T> voltage)
            {
                if (network.links.TryGet(linkId).ThenDont(out LinkCache.Link? found))
                {
                    linkDrives?.Remove(linkId);
                    return;
                }

                if (found is not LinkCache.Link<T>)
                    throw new("Unable to drive link. Input type is not correct");

                network.Push(new VoltagePush<T>(Net<T>.Secrets.FromIntegerCode(Id), linkId, voltage));
            }

            /// <summary>
            /// Causes this node to start driving a link
            /// </summary>
            /// <param name="linkId"></param>
            internal void StartDrivingLink(LinkId linkId)
            {
                if (alreadyDisposed) return;
                linkDrives ??= [];
                if (linkDrives.Add(linkId))
                    LookupAndDriveLink(linkId, GetImmediateSample());
                else
                    throw new($"A voltage sampler with id {linkId} already exists on net {Id}");
            }

            /// <summary>
            /// Removes this link right before sending it a HiZ
            /// </summary>
            /// <param name="linkId"></param>
            /// <returns></returns>
            internal bool StopDrivingLink(LinkId linkId)
            {
                if (alreadyDisposed) return false;
                if (linkDrives is null) return false;
                if (linkDrives.Remove(linkId))
                {
                    LookupAndDriveLink(linkId, default);
                    return true;
                }
                else return false;
            }

            internal void TakeInput(LinkId sender, Voltage<T> voltage)
            {
                if (alreadyDisposed) return; // If retired, stop updating.

                // Check if the incoming magnitude is HiZ. That means remove the sender
                if (voltage.Magnitude is L3.Z)
                {
                    if (!inputs.Remove(sender))
                        return; // If we didn't actually remove anything, cancel
                }
                else
                {
                    // incoming voltage was not HiZ.
                    if (inputs.TryGetValue(sender, out Voltage<T> replacement) && replacement == voltage)
                        return; // If the incoming voltage from this sender already exists, do nothing
                    else
                        inputs[sender] = voltage; // Otherwise, accept the new value and continue
                }

                // Recompute output value
                switch (inputs.Count)
                {
                    // No driven values. Output HiZ
                    case 0:
                        PusherSrc = default;
                        State = L6.Z;
                        goto SendPush;
                    // One driven value. Output it.
                    case 1:
                        var onlyEntry = inputs.Single();
                        PusherSrc = onlyEntry.Key;
                        State = onlyEntry.Value.Magnitude switch
                        {
                            L3.S => L6.S,
                            L3.W => L6.W,
                            _ => throw new Exception("This should not happen")
                        };
                        goto SendPush;
                    // Multiple driven values. Perform resolution algorithm.
                    default:
                        State = L6.U;
                        foreach (var entry in inputs)
                        {
                            switch (entry.Value.Magnitude, State)
                            {
                                case (L3.S, L6.S):
                                    if (entry.Value == GetImmediateSample())
                                        continue; // Don't do anything if they actually agree.
                                    // Otherwise, enter hazard state
                                    PusherSrc = default;
                                    State = L6.X;
                                    goto SendPush; // Resolution is concluded. Hazard + anything is hazard
                                case (L3.S, L6.X):
                                    continue; // Still a hazard. Arguably even more hazardous.
                                // Strong overrides all other values
                                case (L3.S, L6.N):
                                case (L3.S, L6.W):
                                case (L3.S, L6.Z):
                                case (L3.S, L6.U):
                                    PusherSrc = entry.Key;
                                    State = L6.S;
                                    continue;

                                // Weak fails to override strong, hazard, and noise
                                case (L3.W, L6.S):
                                case (L3.W, L6.X):
                                case (L3.W, L6.N):
                                    continue;
                                // Two weaks cause noise if they disagree.
                                case (L3.W, L6.W):
                                    if (entry.Value == GetImmediateSample())
                                        continue; // Don't do anything if they actually agree.
                                    PusherSrc = default;
                                    State = L6.N;
                                    continue;
                                // Weak can overpower HiZ and uninit
                                case (L3.W, L6.Z):
                                case (L3.W, L6.U):
                                    PusherSrc = entry.Key;
                                    State = L6.W;
                                    continue;
                                default:
                                    throw new Exception("This also should not happen");
                            }

                        }
                        break;
                }

            SendPush:
                Voltage<T> push = GetImmediateSample();
                network.LogNodeChange(this, push.RawValue());
                PushCount++;
                if (linkDrives is not null)
                    foreach (LinkId link in linkDrives)
                        LookupAndDriveLink(link, push);
                PushCount--;
            }

            public sealed override void Dispose()
            {
                base.Dispose();
                if (alreadyDisposed) return;
                alreadyDisposed = true; // Prevents an outside update from updating this node
                network.LogNodeDestruction(this);
                inputs.Clear();
                if (linkDrives is not null)
                {
                    foreach (LinkId link in linkDrives.ToArray())
                        StopDrivingLink(link);
                }
                linkDrives = null;
            }
        }
    }
}