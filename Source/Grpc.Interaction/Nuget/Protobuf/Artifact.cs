// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: dolittle/interaction/events.relativity/artifact.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Dolittle.Runtime.Grpc.Interaction {

  /// <summary>Holder for reflection information generated from dolittle/interaction/events.relativity/artifact.proto</summary>
  public static partial class ArtifactReflection {

    #region Descriptor
    /// <summary>File descriptor for dolittle/interaction/events.relativity/artifact.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ArtifactReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CjVkb2xpdHRsZS9pbnRlcmFjdGlvbi9ldmVudHMucmVsYXRpdml0eS9hcnRp",
            "ZmFjdC5wcm90bxIaZG9saXR0bGUuZXZlbnRzLnJlbGF0aXZpdHkaEXN5c3Rl",
            "bS9ndWlkLnByb3RvIjoKCEFydGlmYWN0EhoKAmlkGAEgASgLMg4uZG9saXR0",
            "bGUuZ3VpZBISCgpnZW5lcmF0aW9uGAIgASgFQiSqAiFEb2xpdHRsZS5SdW50",
            "aW1lLkdycGMuSW50ZXJhY3Rpb25iBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::System.Protobuf.GuidReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Dolittle.Runtime.Grpc.Interaction.Artifact), global::Dolittle.Runtime.Grpc.Interaction.Artifact.Parser, new[]{ "Id", "Generation" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// Represents the artifact
  /// </summary>
  public sealed partial class Artifact : pb::IMessage<Artifact> {
    private static readonly pb::MessageParser<Artifact> _parser = new pb::MessageParser<Artifact>(() => new Artifact());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Artifact> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Dolittle.Runtime.Grpc.Interaction.ArtifactReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Artifact() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Artifact(Artifact other) : this() {
      Id = other.id_ != null ? other.Id.Clone() : null;
      generation_ = other.generation_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Artifact Clone() {
      return new Artifact(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private global::System.Protobuf.guid id_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::System.Protobuf.guid Id {
      get { return id_; }
      set {
        id_ = value;
      }
    }

    /// <summary>Field number for the "generation" field.</summary>
    public const int GenerationFieldNumber = 2;
    private int generation_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Generation {
      get { return generation_; }
      set {
        generation_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Artifact);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Artifact other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Id, other.Id)) return false;
      if (Generation != other.Generation) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (id_ != null) hash ^= Id.GetHashCode();
      if (Generation != 0) hash ^= Generation.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (id_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Id);
      }
      if (Generation != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Generation);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (id_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Id);
      }
      if (Generation != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Generation);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Artifact other) {
      if (other == null) {
        return;
      }
      if (other.id_ != null) {
        if (id_ == null) {
          id_ = new global::System.Protobuf.guid();
        }
        Id.MergeFrom(other.Id);
      }
      if (other.Generation != 0) {
        Generation = other.Generation;
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
            if (id_ == null) {
              id_ = new global::System.Protobuf.guid();
            }
            input.ReadMessage(id_);
            break;
          }
          case 16: {
            Generation = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code