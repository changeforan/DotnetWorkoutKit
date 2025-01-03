// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: CustomWorkout/WorkoutStep.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace DotnetWorkoutKit.Protobuf.CustomWorkout {

  /// <summary>Holder for reflection information generated from CustomWorkout/WorkoutStep.proto</summary>
  internal static partial class WorkoutStepReflection {

    #region Descriptor
    /// <summary>File descriptor for CustomWorkout/WorkoutStep.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static WorkoutStepReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Ch9DdXN0b21Xb3Jrb3V0L1dvcmtvdXRTdGVwLnByb3RvGhhBbGVydC9Xb3Jr",
            "b3V0QWxlcnQucHJvdG8aEVdvcmtvdXRHb2FsLnByb3RvIpoBCgtXb3Jrb3V0",
            "U3RlcBIiCgx3b3Jrb3V0X2dvYWwYASABKAsyDC5Xb3Jrb3V0R29hbBIpCg13",
            "b3Jrb3V0X2FsZXJ0GAIgASgLMg0uV29ya291dEFsZXJ0SACIAQESGQoMZGlz",
            "cGxheV9uYW1lGAMgASgJSAGIAQFCEAoOX3dvcmtvdXRfYWxlcnRCDwoNX2Rp",
            "c3BsYXlfbmFtZUIqqgInRG90bmV0V29ya291dEtpdC5Qcm90b2J1Zi5DdXN0",
            "b21Xb3Jrb3V0YgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::DotnetWorkoutKit.Protobuf.CustomWorkout.Alert.WorkoutAlertReflection.Descriptor, global::DotnetWorkoutKit.Protobuf.CustomWorkout.WorkoutGoalReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::DotnetWorkoutKit.Protobuf.CustomWorkout.WorkoutStep), global::DotnetWorkoutKit.Protobuf.CustomWorkout.WorkoutStep.Parser, new[]{ "WorkoutGoal", "WorkoutAlert", "DisplayName" }, new[]{ "WorkoutAlert", "DisplayName" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  [global::System.Diagnostics.DebuggerDisplayAttribute("{ToString(),nq}")]
  internal sealed partial class WorkoutStep : pb::IMessage<WorkoutStep>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<WorkoutStep> _parser = new pb::MessageParser<WorkoutStep>(() => new WorkoutStep());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<WorkoutStep> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::DotnetWorkoutKit.Protobuf.CustomWorkout.WorkoutStepReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public WorkoutStep() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public WorkoutStep(WorkoutStep other) : this() {
      workoutGoal_ = other.workoutGoal_ != null ? other.workoutGoal_.Clone() : null;
      workoutAlert_ = other.workoutAlert_ != null ? other.workoutAlert_.Clone() : null;
      displayName_ = other.displayName_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public WorkoutStep Clone() {
      return new WorkoutStep(this);
    }

    /// <summary>Field number for the "workout_goal" field.</summary>
    public const int WorkoutGoalFieldNumber = 1;
    private global::DotnetWorkoutKit.Protobuf.CustomWorkout.WorkoutGoal workoutGoal_;
    /// <summary>
    /// WorkoutGoal
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::DotnetWorkoutKit.Protobuf.CustomWorkout.WorkoutGoal WorkoutGoal {
      get { return workoutGoal_; }
      set {
        workoutGoal_ = value;
      }
    }

    /// <summary>Field number for the "workout_alert" field.</summary>
    public const int WorkoutAlertFieldNumber = 2;
    private global::DotnetWorkoutKit.Protobuf.CustomWorkout.Alert.WorkoutAlert workoutAlert_;
    /// <summary>
    /// WorkoutAlert (optional)
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public global::DotnetWorkoutKit.Protobuf.CustomWorkout.Alert.WorkoutAlert WorkoutAlert {
      get { return workoutAlert_; }
      set {
        workoutAlert_ = value;
      }
    }

    /// <summary>Field number for the "display_name" field.</summary>
    public const int DisplayNameFieldNumber = 3;
    private readonly static string DisplayNameDefaultValue = "";

    private string displayName_;
    /// <summary>
    /// DisplayName (optional)
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string DisplayName {
      get { return displayName_ ?? DisplayNameDefaultValue; }
      set {
        displayName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "display_name" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasDisplayName {
      get { return displayName_ != null; }
    }
    /// <summary>Clears the value of the "display_name" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearDisplayName() {
      displayName_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as WorkoutStep);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(WorkoutStep other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(WorkoutGoal, other.WorkoutGoal)) return false;
      if (!object.Equals(WorkoutAlert, other.WorkoutAlert)) return false;
      if (DisplayName != other.DisplayName) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (workoutGoal_ != null) hash ^= WorkoutGoal.GetHashCode();
      if (workoutAlert_ != null) hash ^= WorkoutAlert.GetHashCode();
      if (HasDisplayName) hash ^= DisplayName.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (workoutGoal_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(WorkoutGoal);
      }
      if (workoutAlert_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(WorkoutAlert);
      }
      if (HasDisplayName) {
        output.WriteRawTag(26);
        output.WriteString(DisplayName);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (workoutGoal_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(WorkoutGoal);
      }
      if (workoutAlert_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(WorkoutAlert);
      }
      if (HasDisplayName) {
        output.WriteRawTag(26);
        output.WriteString(DisplayName);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (workoutGoal_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(WorkoutGoal);
      }
      if (workoutAlert_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(WorkoutAlert);
      }
      if (HasDisplayName) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(DisplayName);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(WorkoutStep other) {
      if (other == null) {
        return;
      }
      if (other.workoutGoal_ != null) {
        if (workoutGoal_ == null) {
          WorkoutGoal = new global::DotnetWorkoutKit.Protobuf.CustomWorkout.WorkoutGoal();
        }
        WorkoutGoal.MergeFrom(other.WorkoutGoal);
      }
      if (other.workoutAlert_ != null) {
        if (workoutAlert_ == null) {
          WorkoutAlert = new global::DotnetWorkoutKit.Protobuf.CustomWorkout.Alert.WorkoutAlert();
        }
        WorkoutAlert.MergeFrom(other.WorkoutAlert);
      }
      if (other.HasDisplayName) {
        DisplayName = other.DisplayName;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (workoutGoal_ == null) {
              WorkoutGoal = new global::DotnetWorkoutKit.Protobuf.CustomWorkout.WorkoutGoal();
            }
            input.ReadMessage(WorkoutGoal);
            break;
          }
          case 18: {
            if (workoutAlert_ == null) {
              WorkoutAlert = new global::DotnetWorkoutKit.Protobuf.CustomWorkout.Alert.WorkoutAlert();
            }
            input.ReadMessage(WorkoutAlert);
            break;
          }
          case 26: {
            DisplayName = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
      if ((tag & 7) == 4) {
        // Abort on any end group tag.
        return;
      }
      switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (workoutGoal_ == null) {
              WorkoutGoal = new global::DotnetWorkoutKit.Protobuf.CustomWorkout.WorkoutGoal();
            }
            input.ReadMessage(WorkoutGoal);
            break;
          }
          case 18: {
            if (workoutAlert_ == null) {
              WorkoutAlert = new global::DotnetWorkoutKit.Protobuf.CustomWorkout.Alert.WorkoutAlert();
            }
            input.ReadMessage(WorkoutAlert);
            break;
          }
          case 26: {
            DisplayName = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
