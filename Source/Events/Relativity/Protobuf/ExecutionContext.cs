// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: dolittle/interaction/events.relativity/execution_context.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Dolittle.Runtime.Events.Relativity.Protobuf {

  /// <summary>Holder for reflection information generated from dolittle/interaction/events.relativity/execution_context.proto</summary>
  public static partial class ExecutionContextReflection {

    #region Descriptor
    /// <summary>File descriptor for dolittle/interaction/events.relativity/execution_context.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ExecutionContextReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cj5kb2xpdHRsZS9pbnRlcmFjdGlvbi9ldmVudHMucmVsYXRpdml0eS9leGVj",
            "dXRpb25fY29udGV4dC5wcm90bxIaZG9saXR0bGUuZXZlbnRzLnJlbGF0aXZp",
            "dHkaEXN5c3RlbS9ndWlkLnByb3RvGjJkb2xpdHRsZS9pbnRlcmFjdGlvbi9l",
            "dmVudHMucmVsYXRpdml0eS9jbGFpbS5wcm90byL/AQoQRXhlY3V0aW9uQ29u",
            "dGV4dBIjCgthcHBsaWNhdGlvbhgBIAEoCzIOLmRvbGl0dGxlLmd1aWQSJgoO",
            "Ym91bmRlZENvbnRleHQYAiABKAsyDi5kb2xpdHRsZS5ndWlkEh4KBnRlbmFu",
            "dBgDIAEoCzIOLmRvbGl0dGxlLmd1aWQSJQoNY29ycmVsYXRpb25JZBgEIAEo",
            "CzIOLmRvbGl0dGxlLmd1aWQSEwoLZW52aXJvbm1lbnQYBSABKAkSMQoGY2xh",
            "aW1zGAYgAygLMiEuZG9saXR0bGUuZXZlbnRzLnJlbGF0aXZpdHkuQ2xhaW0S",
            "DwoHY3VsdHVyZRgHIAEoCUIuqgIrRG9saXR0bGUuUnVudGltZS5FdmVudHMu",
            "UmVsYXRpdml0eS5Qcm90b2J1ZmIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::System.Protobuf.GuidReflection.Descriptor, global::Dolittle.Runtime.Events.Relativity.Protobuf.ClaimReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Dolittle.Runtime.Events.Relativity.Protobuf.ExecutionContext), global::Dolittle.Runtime.Events.Relativity.Protobuf.ExecutionContext.Parser, new[]{ "Application", "BoundedContext", "Tenant", "CorrelationId", "Environment", "Claims", "Culture" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// Represents the execution context
  /// </summary>
  public sealed partial class ExecutionContext : pb::IMessage<ExecutionContext> {
    private static readonly pb::MessageParser<ExecutionContext> _parser = new pb::MessageParser<ExecutionContext>(() => new ExecutionContext());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ExecutionContext> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Dolittle.Runtime.Events.Relativity.Protobuf.ExecutionContextReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExecutionContext() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExecutionContext(ExecutionContext other) : this() {
      Application = other.application_ != null ? other.Application.Clone() : null;
      BoundedContext = other.boundedContext_ != null ? other.BoundedContext.Clone() : null;
      Tenant = other.tenant_ != null ? other.Tenant.Clone() : null;
      CorrelationId = other.correlationId_ != null ? other.CorrelationId.Clone() : null;
      environment_ = other.environment_;
      claims_ = other.claims_.Clone();
      culture_ = other.culture_;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ExecutionContext Clone() {
      return new ExecutionContext(this);
    }

    /// <summary>Field number for the "application" field.</summary>
    public const int ApplicationFieldNumber = 1;
    private global::System.Protobuf.guid application_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::System.Protobuf.guid Application {
      get { return application_; }
      set {
        application_ = value;
      }
    }

    /// <summary>Field number for the "boundedContext" field.</summary>
    public const int BoundedContextFieldNumber = 2;
    private global::System.Protobuf.guid boundedContext_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::System.Protobuf.guid BoundedContext {
      get { return boundedContext_; }
      set {
        boundedContext_ = value;
      }
    }

    /// <summary>Field number for the "tenant" field.</summary>
    public const int TenantFieldNumber = 3;
    private global::System.Protobuf.guid tenant_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::System.Protobuf.guid Tenant {
      get { return tenant_; }
      set {
        tenant_ = value;
      }
    }

    /// <summary>Field number for the "correlationId" field.</summary>
    public const int CorrelationIdFieldNumber = 4;
    private global::System.Protobuf.guid correlationId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::System.Protobuf.guid CorrelationId {
      get { return correlationId_; }
      set {
        correlationId_ = value;
      }
    }

    /// <summary>Field number for the "environment" field.</summary>
    public const int EnvironmentFieldNumber = 5;
    private string environment_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Environment {
      get { return environment_; }
      set {
        environment_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "claims" field.</summary>
    public const int ClaimsFieldNumber = 6;
    private static readonly pb::FieldCodec<global::Dolittle.Runtime.Events.Relativity.Protobuf.Claim> _repeated_claims_codec
        = pb::FieldCodec.ForMessage(50, global::Dolittle.Runtime.Events.Relativity.Protobuf.Claim.Parser);
    private readonly pbc::RepeatedField<global::Dolittle.Runtime.Events.Relativity.Protobuf.Claim> claims_ = new pbc::RepeatedField<global::Dolittle.Runtime.Events.Relativity.Protobuf.Claim>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Dolittle.Runtime.Events.Relativity.Protobuf.Claim> Claims {
      get { return claims_; }
    }

    /// <summary>Field number for the "culture" field.</summary>
    public const int CultureFieldNumber = 7;
    private string culture_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Culture {
      get { return culture_; }
      set {
        culture_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ExecutionContext);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ExecutionContext other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Application, other.Application)) return false;
      if (!object.Equals(BoundedContext, other.BoundedContext)) return false;
      if (!object.Equals(Tenant, other.Tenant)) return false;
      if (!object.Equals(CorrelationId, other.CorrelationId)) return false;
      if (Environment != other.Environment) return false;
      if(!claims_.Equals(other.claims_)) return false;
      if (Culture != other.Culture) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (application_ != null) hash ^= Application.GetHashCode();
      if (boundedContext_ != null) hash ^= BoundedContext.GetHashCode();
      if (tenant_ != null) hash ^= Tenant.GetHashCode();
      if (correlationId_ != null) hash ^= CorrelationId.GetHashCode();
      if (Environment.Length != 0) hash ^= Environment.GetHashCode();
      hash ^= claims_.GetHashCode();
      if (Culture.Length != 0) hash ^= Culture.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (application_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Application);
      }
      if (boundedContext_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(BoundedContext);
      }
      if (tenant_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(Tenant);
      }
      if (correlationId_ != null) {
        output.WriteRawTag(34);
        output.WriteMessage(CorrelationId);
      }
      if (Environment.Length != 0) {
        output.WriteRawTag(42);
        output.WriteString(Environment);
      }
      claims_.WriteTo(output, _repeated_claims_codec);
      if (Culture.Length != 0) {
        output.WriteRawTag(58);
        output.WriteString(Culture);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (application_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Application);
      }
      if (boundedContext_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(BoundedContext);
      }
      if (tenant_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Tenant);
      }
      if (correlationId_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(CorrelationId);
      }
      if (Environment.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Environment);
      }
      size += claims_.CalculateSize(_repeated_claims_codec);
      if (Culture.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Culture);
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ExecutionContext other) {
      if (other == null) {
        return;
      }
      if (other.application_ != null) {
        if (application_ == null) {
          application_ = new global::System.Protobuf.guid();
        }
        Application.MergeFrom(other.Application);
      }
      if (other.boundedContext_ != null) {
        if (boundedContext_ == null) {
          boundedContext_ = new global::System.Protobuf.guid();
        }
        BoundedContext.MergeFrom(other.BoundedContext);
      }
      if (other.tenant_ != null) {
        if (tenant_ == null) {
          tenant_ = new global::System.Protobuf.guid();
        }
        Tenant.MergeFrom(other.Tenant);
      }
      if (other.correlationId_ != null) {
        if (correlationId_ == null) {
          correlationId_ = new global::System.Protobuf.guid();
        }
        CorrelationId.MergeFrom(other.CorrelationId);
      }
      if (other.Environment.Length != 0) {
        Environment = other.Environment;
      }
      claims_.Add(other.claims_);
      if (other.Culture.Length != 0) {
        Culture = other.Culture;
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
            if (application_ == null) {
              application_ = new global::System.Protobuf.guid();
            }
            input.ReadMessage(application_);
            break;
          }
          case 18: {
            if (boundedContext_ == null) {
              boundedContext_ = new global::System.Protobuf.guid();
            }
            input.ReadMessage(boundedContext_);
            break;
          }
          case 26: {
            if (tenant_ == null) {
              tenant_ = new global::System.Protobuf.guid();
            }
            input.ReadMessage(tenant_);
            break;
          }
          case 34: {
            if (correlationId_ == null) {
              correlationId_ = new global::System.Protobuf.guid();
            }
            input.ReadMessage(correlationId_);
            break;
          }
          case 42: {
            Environment = input.ReadString();
            break;
          }
          case 50: {
            claims_.AddEntriesFrom(input, _repeated_claims_codec);
            break;
          }
          case 58: {
            Culture = input.ReadString();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code