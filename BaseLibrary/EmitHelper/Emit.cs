using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace lpp.EmitHelper
{
    public sealed class Emit
    {
        /// <summary>
        /// Ldind 间接加载。（即从地址加载）
        /// </summary>       
        public static void Ldind(ILGenerator ilGenerator, Type type)
        {
            if (!type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Ldind_Ref);
                return;
            }

            if (type.IsEnum)
            {
                Type underType = Enum.GetUnderlyingType(type);
                Emit.Ldind(ilGenerator, underType);
                return;
            }

            if (type == typeof(Int64))
            {
                ilGenerator.Emit(OpCodes.Ldind_I8);
                return;
            }

            if (type == typeof(Int32))
            {
                ilGenerator.Emit(OpCodes.Ldind_I4);
                return;
            }

            if (type == typeof(Int16))
            {
                ilGenerator.Emit(OpCodes.Ldind_I2);
                return;
            }

            if (type == typeof(Byte))
            {
                ilGenerator.Emit(OpCodes.Ldind_U1);
                return;
            }

            if (type == typeof(SByte))
            {
                ilGenerator.Emit(OpCodes.Ldind_I1);
                return;
            }

            if (type == typeof(Boolean))
            {
                ilGenerator.Emit(OpCodes.Ldind_I1);
                return;
            }

            if (type == typeof(UInt64))
            {
                ilGenerator.Emit(OpCodes.Ldind_I8);
                return;
            }

            if (type == typeof(UInt32))
            {
                ilGenerator.Emit(OpCodes.Ldind_U4);
                return;
            }

            if (type == typeof(UInt16))
            {
                ilGenerator.Emit(OpCodes.Ldind_U2);
                return;
            }

            if (type == typeof(Single))
            {
                ilGenerator.Emit(OpCodes.Ldind_R4);
                return;
            }

            if (type == typeof(Double))
            {
                ilGenerator.Emit(OpCodes.Ldind_R8);
                return;
            }

            if (type == typeof(System.IntPtr))
            {
                ilGenerator.Emit(OpCodes.Ldind_I4);
                return;
            }

            if (type == typeof(System.UIntPtr))
            {
                ilGenerator.Emit(OpCodes.Ldind_I4);
                return;
            }

            throw new Exception(string.Format("The target type:{0} is not supported by EmitHelper.Ldind()", type));
        }

        /// <summary>
        /// Stind 间接存储
        /// </summary>      
        public static void Stind(ILGenerator ilGenerator, Type type)
        {
            if (!type.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Stind_Ref);
                return;
            }

            if (type.IsEnum)
            {
                Type underType = Enum.GetUnderlyingType(type);
                Emit.Stind(ilGenerator, underType);
                return;
            }

            if (type == typeof(Int64))
            {
                ilGenerator.Emit(OpCodes.Stind_I8);
                return;
            }

            if (type == typeof(Int32))
            {
                ilGenerator.Emit(OpCodes.Stind_I4);
                return;
            }

            if (type == typeof(Int16))
            {
                ilGenerator.Emit(OpCodes.Stind_I2);
                return;
            }

            if (type == typeof(Byte))
            {
                ilGenerator.Emit(OpCodes.Stind_I1);
                return;
            }

            if (type == typeof(SByte))
            {
                ilGenerator.Emit(OpCodes.Stind_I1);
                return;
            }

            if (type == typeof(Boolean))
            {
                ilGenerator.Emit(OpCodes.Stind_I1);
                return;
            }

            if (type == typeof(UInt64))
            {
                ilGenerator.Emit(OpCodes.Stind_I8);
                return;
            }

            if (type == typeof(UInt32))
            {
                ilGenerator.Emit(OpCodes.Stind_I4);
                return;
            }

            if (type == typeof(UInt16))
            {
                ilGenerator.Emit(OpCodes.Stind_I2);
                return;
            }

            if (type == typeof(Single))
            {
                ilGenerator.Emit(OpCodes.Stind_R4);
                return;
            }

            if (type == typeof(Double))
            {
                ilGenerator.Emit(OpCodes.Stind_R8);
                return;
            }

            if (type == typeof(System.IntPtr))
            {
                ilGenerator.Emit(OpCodes.Stind_I4);
                return;
            }

            if (type == typeof(System.UIntPtr))
            {
                ilGenerator.Emit(OpCodes.Stind_I4);
                return;
            }

            throw new Exception(string.Format("The target type:{0} is not supported by EmitHelper.Stind_ForValueType()", type));
        } 
    }
}
