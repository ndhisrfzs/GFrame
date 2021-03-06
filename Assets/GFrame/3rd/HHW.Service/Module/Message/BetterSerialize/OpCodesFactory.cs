﻿#if Dynamic
using System;
using System.Reflection.Emit;

namespace HHW.Service
{
    internal static class OpCodesFactory
    {
        /// <summary>
        /// 压入整数堆栈方法
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static OpCode GetLdc_I4(int index)
        {
            if (index == 0)
                return OpCodes.Ldc_I4_0;
            if (index == 1)
                return OpCodes.Ldc_I4_1;
            if (index == 2)
                return OpCodes.Ldc_I4_2;
            if (index == 3)
                return OpCodes.Ldc_I4_3;
            if (index == 4)
                return OpCodes.Ldc_I4_4;
            if (index == 5)
                return OpCodes.Ldc_I4_5;
            if (index == 6)
                return OpCodes.Ldc_I4_6;
            if (index == 7)
                return OpCodes.Ldc_I4_7;
            if (index == 8)
                return OpCodes.Ldc_I4_8;
            if (index > 8)
                return OpCodes.Ldc_I4_S;

            throw new Exception("unknown index to ldc_i4");
        }

        ///// <summary>
        ///// 根据传入的数据类型，获取数组元素的op
        ///// </summary>
        ///// <param name="reflectType"></param>
        ///// <returns></returns>
        //public static OpCode GetLdelem(Type reflectType)
        //{
        //    if (!reflectType.IsValueType)
        //        return OpCodes.Ldelem_Ref;

        //    if (typeof(byte).Equals(reflectType))
        //        return OpCodes.Ldelem_I1;

        //    if (typeof(short).Equals(reflectType))
        //        return OpCodes.Ldelem_I2;

        //    if (typeof(Int32).Equals(reflectType))
        //        return OpCodes.Ldelem_I4;

        //    if (typeof(Int64).Equals(reflectType))
        //        return OpCodes.Ldelem_I8;

        //    if (typeof(float).Equals(reflectType))
        //        return OpCodes.Ldelem_R4;

        //    if (typeof(double).Equals(reflectType))
        //        return OpCodes.Ldelem_R8;

        //    return OpCodes.Ldelem_Ref;
        //}

        /// <summary>
        /// 判断是否需要装箱
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type"></param>
        public static void BoxIfNeeded(ILGenerator generator, Type type)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Box, type);
            }
        }

        /// <summary>
        /// 判断是否需要拆箱
        /// </summary>
        /// <param name="generator"></param>
        /// <param name="type"></param>
        public static void UnboxIfNeeded(ILGenerator generator, Type type)
        {
            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, type);
            }
        }




        public static void EmitFastInt(ILGenerator il, int value)
        {
            switch (value)
            {
                case -1:
                    il.Emit(OpCodes.Ldc_I4_M1);
                    return;
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    return;
                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    return;
                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    return;
                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    return;
                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    return;
                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    return;
                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    return;
                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    return;
                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    return;
            }

            if (value > -129 && value < 128)
            {
                il.Emit(OpCodes.Ldc_I4_S, (SByte)value);
            }
            else
            {
                il.Emit(OpCodes.Ldc_I4, value);
            }
        }


        public static void EmitCastToReference(ILGenerator il, System.Type type)
        {
            if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }

    }
}
#endif
