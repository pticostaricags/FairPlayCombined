#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable S2292 // Trivial properties should be auto-implemented
using System;
using System.Collections.Generic;
using System.Text;

namespace FairPlayCombined.Models.Generators
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02", IsNullable = false)]
    public partial class DataSchemaModel
    {

        private DataSchemaModelCustomData[] headerField;

        private DataSchemaModelElement[] modelField;

        private decimal fileFormatVersionField;

        private decimal schemaVersionField;

        private string dspNameField;

        private ushort collationLcidField;

        private string collationCaseSensitiveField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("CustomData", IsNullable = false)]
        public DataSchemaModelCustomData[] Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Element", IsNullable = false)]
        public DataSchemaModelElement[] Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal FileFormatVersion
        {
            get
            {
                return this.fileFormatVersionField;
            }
            set
            {
                this.fileFormatVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal SchemaVersion
        {
            get
            {
                return this.schemaVersionField;
            }
            set
            {
                this.schemaVersionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string DspName
        {
            get
            {
                return this.dspNameField;
            }
            set
            {
                this.dspNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort CollationLcid
        {
            get
            {
                return this.collationLcidField;
            }
            set
            {
                this.collationLcidField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CollationCaseSensitive
        {
            get
            {
                return this.collationCaseSensitiveField;
            }
            set
            {
                this.collationCaseSensitiveField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelCustomData
    {

        private DataSchemaModelCustomDataMetadata[] metadataField;

        private string categoryField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Metadata")]
        public DataSchemaModelCustomDataMetadata[] Metadata
        {
            get
            {
                return this.metadataField;
            }
            set
            {
                this.metadataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelCustomDataMetadata
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElement
    {

        private DataSchemaModelElementProperty[] propertyField;

        private DataSchemaModelElementRelationship[] relationshipField;

        private DataSchemaModelElementAttachedAnnotation[] attachedAnnotationField;

        private DataSchemaModelElementAnnotation[] annotationField;

        private string typeField;

        private string nameField;

        private byte disambiguatorField;

        private bool disambiguatorFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property")]
        public DataSchemaModelElementProperty[] Property
        {
            get
            {
                return this.propertyField;
            }
            set
            {
                this.propertyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Relationship")]
        public DataSchemaModelElementRelationship[] Relationship
        {
            get
            {
                return this.relationshipField;
            }
            set
            {
                this.relationshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("AttachedAnnotation")]
        public DataSchemaModelElementAttachedAnnotation[] AttachedAnnotation
        {
            get
            {
                return this.attachedAnnotationField;
            }
            set
            {
                this.attachedAnnotationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Annotation")]
        public DataSchemaModelElementAnnotation[] Annotation
        {
            get
            {
                return this.annotationField;
            }
            set
            {
                this.annotationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Disambiguator
        {
            get
            {
                return this.disambiguatorField;
            }
            set
            {
                this.disambiguatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DisambiguatorSpecified
        {
            get
            {
                return this.disambiguatorFieldSpecified;
            }
            set
            {
                this.disambiguatorFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementProperty
    {

        private string valueField;

        private string nameField;

        private string value1Field;

        /// <remarks/>
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("Value")]
        public string Value1
        {
            get
            {
                return this.value1Field;
            }
            set
            {
                this.value1Field = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationship
    {

        private DataSchemaModelElementRelationshipEntry[] entryField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Entry")]
        public DataSchemaModelElementRelationshipEntry[] Entry
        {
            get
            {
                return this.entryField;
            }
            set
            {
                this.entryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntry
    {

        private DataSchemaModelElementRelationshipEntryElement elementField;

        private DataSchemaModelElementRelationshipEntryReferences referencesField;

        /// <remarks/>
        public DataSchemaModelElementRelationshipEntryElement Element
        {
            get
            {
                return this.elementField;
            }
            set
            {
                this.elementField = value;
            }
        }

        /// <remarks/>
        public DataSchemaModelElementRelationshipEntryReferences References
        {
            get
            {
                return this.referencesField;
            }
            set
            {
                this.referencesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElement
    {

        private DataSchemaModelElementRelationshipEntryElementProperty[] propertyField;

        private DataSchemaModelElementRelationshipEntryElementRelationship relationshipField;

        private DataSchemaModelElementRelationshipEntryElementAttachedAnnotation attachedAnnotationField;

        private DataSchemaModelElementRelationshipEntryElementAnnotation annotationField;

        private string typeField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property")]
        public DataSchemaModelElementRelationshipEntryElementProperty[] Property
        {
            get
            {
                return this.propertyField;
            }
            set
            {
                this.propertyField = value;
            }
        }

        /// <remarks/>
        public DataSchemaModelElementRelationshipEntryElementRelationship Relationship
        {
            get
            {
                return this.relationshipField;
            }
            set
            {
                this.relationshipField = value;
            }
        }

        /// <remarks/>
        public DataSchemaModelElementRelationshipEntryElementAttachedAnnotation AttachedAnnotation
        {
            get
            {
                return this.attachedAnnotationField;
            }
            set
            {
                this.attachedAnnotationField = value;
            }
        }

        /// <remarks/>
        public DataSchemaModelElementRelationshipEntryElementAnnotation Annotation
        {
            get
            {
                return this.annotationField;
            }
            set
            {
                this.annotationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElementProperty
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElementRelationship
    {

        private DataSchemaModelElementRelationshipEntryElementRelationshipEntry entryField;

        private string nameField;

        /// <remarks/>
        public DataSchemaModelElementRelationshipEntryElementRelationshipEntry Entry
        {
            get
            {
                return this.entryField;
            }
            set
            {
                this.entryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElementRelationshipEntry
    {

        private DataSchemaModelElementRelationshipEntryElementRelationshipEntryElement elementField;

        private DataSchemaModelElementRelationshipEntryElementRelationshipEntryReferences referencesField;

        /// <remarks/>
        public DataSchemaModelElementRelationshipEntryElementRelationshipEntryElement Element
        {
            get
            {
                return this.elementField;
            }
            set
            {
                this.elementField = value;
            }
        }

        /// <remarks/>
        public DataSchemaModelElementRelationshipEntryElementRelationshipEntryReferences References
        {
            get
            {
                return this.referencesField;
            }
            set
            {
                this.referencesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElementRelationshipEntryElement
    {

        private DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementProperty[] propertyField;

        private DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementRelationship relationshipField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property")]
        public DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementProperty[] Property
        {
            get
            {
                return this.propertyField;
            }
            set
            {
                this.propertyField = value;
            }
        }

        /// <remarks/>
        public DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementRelationship Relationship
        {
            get
            {
                return this.relationshipField;
            }
            set
            {
                this.relationshipField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementProperty
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementRelationship
    {

        private DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementRelationshipEntry entryField;

        private string nameField;

        /// <remarks/>
        public DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementRelationshipEntry Entry
        {
            get
            {
                return this.entryField;
            }
            set
            {
                this.entryField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementRelationshipEntry
    {

        private DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementRelationshipEntryReferences referencesField;

        /// <remarks/>
        public DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementRelationshipEntryReferences References
        {
            get
            {
                return this.referencesField;
            }
            set
            {
                this.referencesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElementRelationshipEntryElementRelationshipEntryReferences
    {

        private string externalSourceField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ExternalSource
        {
            get
            {
                return this.externalSourceField;
            }
            set
            {
                this.externalSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElementRelationshipEntryReferences
    {

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElementAttachedAnnotation
    {

        private byte disambiguatorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Disambiguator
        {
            get
            {
                return this.disambiguatorField;
            }
            set
            {
                this.disambiguatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryElementAnnotation
    {

        private string typeField;

        private byte disambiguatorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Disambiguator
        {
            get
            {
                return this.disambiguatorField;
            }
            set
            {
                this.disambiguatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementRelationshipEntryReferences
    {

        private string externalSourceField;

        private string nameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ExternalSource
        {
            get
            {
                return this.externalSourceField;
            }
            set
            {
                this.externalSourceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementAttachedAnnotation
    {

        private byte disambiguatorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Disambiguator
        {
            get
            {
                return this.disambiguatorField;
            }
            set
            {
                this.disambiguatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementAnnotation
    {

        private DataSchemaModelElementAnnotationProperty[] propertyField;

        private string typeField;

        private byte disambiguatorField;

        private bool disambiguatorFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Property")]
        public DataSchemaModelElementAnnotationProperty[] Property
        {
            get
            {
                return this.propertyField;
            }
            set
            {
                this.propertyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte Disambiguator
        {
            get
            {
                return this.disambiguatorField;
            }
            set
            {
                this.disambiguatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DisambiguatorSpecified
        {
            get
            {
                return this.disambiguatorFieldSpecified;
            }
            set
            {
                this.disambiguatorFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.microsoft.com/sqlserver/dac/Serialization/2012/02")]
    public partial class DataSchemaModelElementAnnotationProperty
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
#pragma warning disable S2292 // Trivial properties should be auto-implemented