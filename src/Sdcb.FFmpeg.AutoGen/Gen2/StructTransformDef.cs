﻿using System.Collections.Generic;

namespace Sdcb.FFmpeg.AutoGen.Gen2
{
    internal record StructTransformDef(ClassCategories ClassCategory, string OldName, string NewName, FieldDef[] FieldDefs, bool AllReadOnly)
        : G2TransformDef(ClassCategory, OldName, NewName, FieldDefs, AllReadOnly)
    {
        protected override string DefinitionLineCode => $"public unsafe partial struct {NewName}";

        protected override IEnumerable<string> GetCommonHeaderCode()
        {
            yield return $"private {OldName}* _ptr;";
            yield return "";
            yield return $"public static implicit operator {OldName}*({NewName}? data)";
            yield return $"    => data.HasValue ? ({OldName}*)data.Value._ptr : null;";
            yield return "";
            yield return $"private {NewName}({OldName}* ptr)";
            yield return "{";
            yield return @"    if (ptr == null)";
            yield return @"    {";
            yield return @"        throw new ArgumentNullException(nameof(ptr));";
            yield return @"    }";
            yield return @"    _ptr = ptr;";
            yield return "}";
            yield return "";
            yield return $"public static {NewName} FromNative({OldName}* ptr) => new {NewName}(ptr);";
            yield return $"public static {NewName} FromNative(IntPtr ptr) => new {NewName}(({OldName}*)ptr);";
            yield return $"internal static {NewName}? FromNativeOrNull({OldName}* ptr)";
            yield return $"    => ptr != null ? new {NewName}?(new {NewName}(ptr)) : null;";
        }
    }
}
