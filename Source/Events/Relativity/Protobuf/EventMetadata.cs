// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: dolittle/interaction/events.relativity/event_metadata.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Dolittle.Runtime.Events.Relativity.Protobuf {

  /// <summary>Holder for reflection information generated from dolittle/interaction/events.relativity/event_metadata.proto</summary>
  public static partial class EventMetadataReflection {

    #region Descriptor
    /// <summary>File descriptor for dolittle/interaction/events.relativity/event_metadata.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EventMetadataReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cjtkb2xpdHRsZS9pbnRlcmFjdGlvbi9ldmVudHMucmVsYXRpdml0eS9ldmVu",
            "dF9tZXRhZGF0YS5wcm90bxIaZG9saXR0bGUuZXZlbnRzLnJlbGF0aXZpdHka",
            "EXN5c3RlbS9ndWlkLnByb3RvGjVkb2xpdHRsZS9pbnRlcmFjdGlvbi9ldmVu",
            "dHMucmVsYXRpdml0eS9hcnRpZmFjdC5wcm90bxpDZG9saXR0bGUvaW50ZXJh",
            "Y3Rpb24vZXZlbnRzLnJlbGF0aXZpdHkvdmVyc2lvbmVkX2V2ZW50X3NvdXJj",
            "ZS5wcm90bxo9ZG9saXR0bGUvaW50ZXJhY3Rpb24vZXZlbnRzLnJlbGF0aXZp",
            "dHkvb3JpZ2luYWxfY29udGV4dC5wcm90byKpAgoNRXZlbnRNZXRhZGF0YRIf",
            "CgdldmVudElkGAEgASgLMg4uZG9saXR0bGUuZ3VpZBJACgZzb3VyY2UYAiAB",
            "KAsyMC5kb2xpdHRsZS5ldmVudHMucmVsYXRpdml0eS5WZXJzaW9uZWRFdmVu",
            "dFNvdXJjZRIlCg1jb3JyZWxhdGlvbklkGAMgASgLMg4uZG9saXR0bGUuZ3Vp",
            "ZBI2CghhcnRpZmFjdBgEIAEoCzIkLmRvbGl0dGxlLmV2ZW50cy5yZWxhdGl2",
            "aXR5LkFydGlmYWN0EhAKCG9jY3VycmVkGAUgASgDEkQKD29yaWdpbmFsQ29u",
            "dGV4dBgGIAEoCzIrLmRvbGl0dGxlLmV2ZW50cy5yZWxhdGl2aXR5Lk9yaWdp",
            "bmFsQ29udGV4dEIuqgIrRG9saXR0bGUuUnVudGltZS5FdmVudHMuUmVsYXRp",
            "dml0eS5Qcm90b2J1ZmIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::System.Protobuf.GuidReflection.Descriptor, global::Dolittle.Runtime.Events.Relativity.Protobuf.ArtifactReflection.Descriptor, global::Dolittle.Runtime.Events.Relativity.Protobuf.VersionedEventSourceReflection.Descriptor, global::Dolittle.Runtime.Events.Relativity.Protobuf.OriginalContextReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Dolittle.Runtime.Events.Relativity.Protobuf.EventMetadata), global::Dolittle.Runtime.Events.Relativity.Protobuf.EventMetadata.Parser, new[]{ "EventId", "Source", "CorrelationId", "Artifact", "Occurred", "OriginalContext" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// Represents the metadata related to an event
  /// </summary>
  public sealed partial class EventMetadata : pb::IMessage<EventMetadata> {
    private static readonly pb::MessageParser<EventMetadata> _parser = new pb::MessageParser<EventMetadata>(() => new EventMetadata());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<EventMetadata> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Dolittle.Runtime.Events.Relativity.Protobuf.EventMetadataReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EventMetadata() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EventMetadata(EventMetadata other) : this() {
      EventId = other.eventId_ != null ? other.EventId.Clone() : null;
      Source = other.source_ != null ? other.Source.Clone() : null;
      CorrelationId = other.correlationId_ != null ? other.CorrelationId.Clone() : null;
      Artifact = other.artifact_ != null ? other.Artifact.Clone() : null;
      occurred_ = other.occurred_;
      OriginalContext = other.originalContext_ != null ? other.OriginalContext.Clone() : null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EventMetadata Clone() {
      return new EventMetadata(this);
    }

    /// <summary>Field number for the "eventId" field.</summary>
    public const int EventIdFieldNumber = 1;
    private global::System.Protobuf.guid eventId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::System.Protobuf.guid EventId {
      get { return eventId_; }
      set {
        eventId_ = value;
      }
    }

    /// <summary>Field number for the "source" field.</summary>
    public const int SourceFieldNumber = 2;
    private global::Dolittle.Runtime.Events.Relativity.Protobuf.VersionedEventSource source_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Dolittle.Runtime.Events.Relativity.Protobuf.VersionedEventSource Source {
      get { return source_; }
      set {
        source_ = value;
      }
    }

    /// <summary>Field number for the "correlationId" field.</summary>
    public const int CorrelationIdFieldNumber = 3;
    private global::System.Protobuf.guid correlationId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::System.Protobuf.guid CorrelationId {
      get { return correlationId_; }
      set {
        correlationId_ = value;
      }
    }

    /// <summary>Field number for the "artifact" field.</summary>
    public const int ArtifactFieldNumber = 4;
    private global::Dolittle.Runtime.Events.Relativity.Protobuf.Artifact artifact_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Dolittle.Runtime.Events.Relativity.Protobuf.Artifact Artifact {
      get { return artifact_; }
      set {
        artifact_ = value;
      }
    }

    /// <summary>Field number for the "occurred" field.</summary>
    public const int OccurredFieldNumber = 5;
    private long occurred_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long Occurred {
      get { return occurred_; }
      set {
        occurred_ = value;
      }
    }

    /// <summary>Field number for the "originalContext" field.</summary>
    public const int OriginalContextFieldNumber = 6;
    private global::Dolittle.Runtime.Events.Relativity.Protobuf.OriginalContext originalContext_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Dolittle.Runtime.Events.Relativity.Protobuf.OriginalContext OriginalContext {
      get { return originalContext_; }
      set {
        originalContext_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as EventMetadata);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(EventMetadata other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(EventId, other.EventId)) return false;
      if (!object.Equals(Source, other.Source)) return false;
      if (!object.Equals(CorrelationId, other.CorrelationId)) return false;
      if (!object.Equals(Artifact, other.Artifact)) return false;
      if (Occurred != other.Occurred) return false;
      if (!object.Equals(OriginalContext, other.OriginalContext)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (eventId_ != null) hash ^= EventId.GetHashCode();
      if (source_ != null) hash ^= Source.GetHashCode();
      if (correlationId_ != null) hash ^= CorrelationId.GetHashCode();
      if (artifact_ != null) hash ^= Artifact.GetHashCode();
      if (Occurred != 0L) hash ^= Occurred.GetHashCode();
      if (originalContext_ != null) hash ^= OriginalContext.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (eventId_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(EventId);
      }
      if (source_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Source);
      }
      if (correlationId_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(CorrelationId);
      }
      if (artifact_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(Artifact);
      }
      if (Occurred != 0L) {
        output.WriteRawTag(40);
        output.WriteInt64(Occurred);
      }
      if (originalContext_ != null) {
        output.WriteRawTag(50);
        output.WriteMessage(OriginalContext);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (eventId_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(EventId);
      }
      if (source_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Source);
      }
      if (correlationId_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(CorrelationId);
      }
      if (artifact_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Artifact);
      }
      if (Occurred != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(Occurred);
      }
      if (originalContext_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(OriginalContext);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(EventMetadata other) {
      if (other == null) {
        return;
      }
      if (other.eventId_ != null) {
        if (eventId_ == null) {
          eventId_ = new global::System.Protobuf.guid();
        }
        EventId.MergeFrom(other.EventId);
      }
      if (other.source_ != null) {
        if (source_ == null) {
          source_ = new global::Dolittle.Runtime.Events.Relativity.Protobuf.VersionedEventSource();
        }
        Source.MergeFrom(other.Source);
      }
      if (other.correlationId_ != null) {
        if (correlationId_ == null) {
          correlationId_ = new global::System.Protobuf.guid();
        }
        CorrelationId.MergeFrom(other.CorrelationId);
      }
      if (other.artifact_ != null) {
        if (artifact_ == null) {
          artifact_ = new global::Dolittle.Runtime.Events.Relativity.Protobuf.Artifact();
        }
        Artifact.MergeFrom(other.Artifact);
      }
      if (other.Occurred != 0L) {
        Occurred = other.Occurred;
      }
      if (other.originalContext_ != null) {
        if (originalContext_ == null) {
          originalContext_ = new global::Dolittle.Runtime.Events.Relativity.Protobuf.OriginalContext();
        }
        OriginalContext.MergeFrom(other.OriginalContext);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            if (eventId_ == null) {
              eventId_ = new global::System.Protobuf.guid();
            }
            input.ReadMessage(eventId_);
            break;
          }
          case 18: {
            if (source_ == null) {
              source_ = new global::Dolittle.Runtime.Events.Relativity.Protobuf.VersionedEventSource();
            }
            input.ReadMessage(source_);
            break;
          }
          case 26: {
            if (correlationId_ == null) {
              correlationId_ = new global::System.Protobuf.guid();
            }
            input.ReadMessage(correlationId_);
            break;
          }
          case 34: {
            if (artifact_ == null) {
              artifact_ = new global::Dolittle.Runtime.Events.Relativity.Protobuf.Artifact();
            }
            input.ReadMessage(artifact_);
            break;
          }
          case 40: {
            Occurred = input.ReadInt64();
            break;
          }
          case 50: {
            if (originalContext_ == null) {
              originalContext_ = new global::Dolittle.Runtime.Events.Relativity.Protobuf.OriginalContext();
            }
            input.ReadMessage(originalContext_);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code