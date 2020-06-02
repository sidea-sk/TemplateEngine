using System;

namespace Docx.DataModel.Factories
{
    public static class SimpleModelFactory
    {
        public static SimpleModel ToSimpleModel(this int value, string name, string format = "")
        {
            return new SimpleModel(name, value.ToString(format));
        }

        public static SimpleModel ToSimpleModel(this string value, string name)
        {
            return new SimpleModel(name, value);
        }

        public static SimpleModel ToSimpleModel(this DateTime value, string name, string format = "")
        {
            return new SimpleModel(name, value.ToString(format));
        }

        public static SimpleModel ToSimpleModel(this decimal value, string name, string format = "")
        {
            return new SimpleModel(name, value.ToString(format));
        }
    }
}
