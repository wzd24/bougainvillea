using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;


namespace EasyMigrator
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("This attribute is for legacy code that is migrating. Use object types or nullable value types for a nullable field, and use NotNullAttribute to override object types for a non-nullable field")]
    [AttributeUsage(AttributeTargets.Property)] public class NullAttribute : Attribute { }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)] public class NotNullAttribute : Attribute { }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class NameAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public NameAttribute(string name) { Name = name; }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbTypeAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string CustomType { get; }

        /// <summary>
        /// 
        /// </summary>
        public DbType? DbType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customType"></param>
        public DbTypeAttribute(string customType) { CustomType = customType; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        public DbTypeAttribute(DbType dbType) { DbType = dbType; }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NoPkAttribute : Attribute { }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class PkAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public bool Clustered { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public PkAttribute() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public PkAttribute(string name) { Name = name; }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoIncAttribute : Attribute, Parsing.Model.IAutoIncrement
    {
        /// <summary>
        /// 
        /// </summary>
        public long Seed { get; }

        /// <summary>
        /// 
        /// </summary>
        public long Step { get; }

        /// <summary>
        /// 
        /// </summary>
        public AutoIncAttribute() : this(1) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        public AutoIncAttribute(long seed) : this(seed, 1) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="step"></param>
        public AutoIncAttribute(long seed, long step) { Seed = seed; Step = step; }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PrecisionAttribute : Attribute, Parsing.Model.IPrecision
    {
        /// <summary>
        /// 
        /// </summary>
        internal Length? DefinedPrecision { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Precision { get; }

        /// <summary>
        /// 
        /// </summary>
        internal Length? DefinedScale { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Scale { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        public PrecisionAttribute(int precision, int scale)
            : this(precision) { Scale = scale; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        public PrecisionAttribute(Length precision, int scale)
            : this(precision) { Scale = scale; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="precision"></param>
        /// <param name="scale"></param>
        public PrecisionAttribute(Length precision, Length scale)
            : this(precision) { DefinedScale = scale; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="precision"></param>
        public PrecisionAttribute(int precision) { Precision = precision; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="precision"></param>
        public PrecisionAttribute(Length precision) { DefinedPrecision = precision; }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FkAttribute : Attribute, Parsing.Model.IForeignKey
    {

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Table { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type TableType { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Column { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal bool OnDeleteWasSet { get; set; }
        private Rule? _onDelete;

        /// <summary>
        /// 
        /// </summary>
        public Rule OnDelete
        {
            get => _onDelete ?? Rule.None;
            set
            {
                _onDelete = value;
                OnDeleteWasSet = true;
            }
        }
        Rule? Parsing.Model.IForeignKey.OnDelete
        {
            get => _onDelete;
            set
            {
                _onDelete = value;
                OnDeleteWasSet = true;
            }
        }

        internal bool OnUpdateWasSet { get; set; }
        private Rule? _onUpdate;

        /// <summary>
        /// 
        /// </summary>
        public Rule OnUpdate
        {
            get => _onUpdate ?? Rule.None;
            set
            {
                _onUpdate = value;
                OnUpdateWasSet = true;
            }
        }
        Rule? Parsing.Model.IForeignKey.OnUpdate
        {
            get => _onUpdate;
            set
            {
                _onUpdate = value;
                OnUpdateWasSet = true;
            }
        }

        internal bool IndexedWasSet { get; set; }
        private bool _indexed;

        /// <summary>
        /// 
        /// </summary>
        public bool Indexed
        {
            get => _indexed;
            set
            {
                _indexed = value;
                IndexedWasSet = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        public FkAttribute(string table) { Table = table; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableType"></param>
        public FkAttribute(Type tableType) { TableType = tableType; }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IndexAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Unique { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Clustered { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string With { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class UniqueAttribute : IndexAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        public UniqueAttribute() { Unique = true; }
    }

    /// <summary>
    /// 
    /// </summary>

    [AttributeUsage(AttributeTargets.Property)]
    public class ClusteredAttribute : UniqueAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        public ClusteredAttribute() { Clustered = true; }
    }


    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AnsiAttribute : Attribute { }


    /// <summary>
    /// 
    /// </summary>
    public enum Length
    {

        /// <summary>
        /// 
        /// </summary>
        Default,

        /// <summary>
        /// 
        /// </summary>
        Short,

        /// <summary>
        /// 
        /// </summary>
        Medium,

        /// <summary>
        /// 
        /// </summary>
        Long,

        /// <summary>
        /// 
        /// </summary>
        Max
    }


    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LengthAttribute : Attribute
    {
        internal Length? DefinedLength { get; }

        /// <summary>
        /// 
        /// </summary>
        public int Length { get; }


        /// <summary>
        /// 
        /// </summary>
        public LengthAttribute(int length) { Length = length; }

        /// <summary>
        /// 
        /// </summary>
        public LengthAttribute(Length length) { DefinedLength = length; }
    }


    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ShortAttribute : LengthAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        public ShortAttribute() : base(EasyMigrator.Length.Short) { }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MediumAttribute : LengthAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        public MediumAttribute() : base(EasyMigrator.Length.Medium) { }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LongAttribute : LengthAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        public LongAttribute() : base(EasyMigrator.Length.Long) { }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MaxAttribute : LengthAttribute
    {
        /// <summary>
        /// 
        /// </summary>

        public MaxAttribute() : base(EasyMigrator.Length.Max) { }
    }


    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class FixedAttribute : LengthAttribute
    {

        /// <summary>
        /// 
        /// </summary>
        public FixedAttribute(int length) : base(length) { }
    }


    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DefaultAttribute : Attribute
    {

        /// <summary>
        /// 
        /// </summary>
        public string Expression { get; }

        /// <summary>
        /// 
        /// </summary>
        public DefaultAttribute(string expression) { Expression = expression; }
    }


    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SparseAttribute : Attribute { }
}
