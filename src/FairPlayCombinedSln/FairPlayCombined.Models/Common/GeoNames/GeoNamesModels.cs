using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.GeoNames
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    public partial class geodata
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
    {

        private geodataNearest? nearestField;

        private geodataOsmtags? osmtagsField;

        private geodataAdminareas? adminareasField;

        private geodataMajor? majorField;

        private string? geocodeField;

        private ulong geonumberField;

        private string? threegeonamesField;

        /// <remarks/>
        public geodataNearest? nearest
        {
            get
            {
                return this.nearestField;
            }
            set
            {
                this.nearestField = value;
            }
        }

        /// <remarks/>
        public geodataOsmtags? osmtags
        {
            get
            {
                return this.osmtagsField;
            }
            set
            {
                this.osmtagsField = value;
            }
        }

        /// <remarks/>
        public geodataAdminareas? adminareas
        {
            get
            {
                return this.adminareasField;
            }
            set
            {
                this.adminareasField = value;
            }
        }

        /// <remarks/>
        public geodataMajor? major
        {
            get
            {
                return this.majorField;
            }
            set
            {
                this.majorField = value;
            }
        }

        /// <remarks/>
        public string? geocode
        {
            get
            {
                return this.geocodeField;
            }
            set
            {
                this.geocodeField = value;
            }
        }

        /// <remarks/>
        public ulong geonumber
        {
            get
            {
                return this.geonumberField;
            }
            set
            {
                this.geonumberField = value;
            }
        }

        /// <remarks/>
        public string? threegeonames
        {
            get
            {
                return this.threegeonamesField;
            }
            set
            {
                this.threegeonamesField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class geodataNearest
    {

        private decimal lattField;

        private decimal longtField;

        private ushort elevationField;

        private string? timezoneField;

        private string? cityField;

        private string? provField;

        private object? regionField;

        private string? stateField;

        private decimal inlattField;

        private decimal inlongtField;

        private string? altgeocodeField;

        private decimal distanceField;

        /// <remarks/>
        public decimal latt
        {
            get
            {
                return this.lattField;
            }
            set
            {
                this.lattField = value;
            }
        }

        /// <remarks/>
        public decimal longt
        {
            get
            {
                return this.longtField;
            }
            set
            {
                this.longtField = value;
            }
        }

        /// <remarks/>
        public ushort elevation
        {
            get
            {
                return this.elevationField;
            }
            set
            {
                this.elevationField = value;
            }
        }

        /// <remarks/>
        public string? timezone
        {
            get
            {
                return this.timezoneField;
            }
            set
            {
                this.timezoneField = value;
            }
        }

        /// <remarks/>
        public string? city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string? prov
        {
            get
            {
                return this.provField;
            }
            set
            {
                this.provField = value;
            }
        }

        /// <remarks/>
        public object? region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        public string? state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public decimal inlatt
        {
            get
            {
                return this.inlattField;
            }
            set
            {
                this.inlattField = value;
            }
        }

        /// <remarks/>
        public decimal inlongt
        {
            get
            {
                return this.inlongtField;
            }
            set
            {
                this.inlongtField = value;
            }
        }

        /// <remarks/>
        public string? altgeocode
        {
            get
            {
                return this.altgeocodeField;
            }
            set
            {
                this.altgeocodeField = value;
            }
        }

        /// <remarks/>
        public decimal distance
        {
            get
            {
                return this.distanceField;
            }
            set
            {
                this.distanceField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class geodataOsmtags
    {

        private string? boundaryField;

        private string? nameField;

        private string? typeField;

        private string? land_areaField;

        private byte admin_levelField;

        private string? stateField;

        private string? borderField;

        private uint idField;

        /// <remarks/>
        public string? boundary
        {
            get
            {
                return this.boundaryField;
            }
            set
            {
                this.boundaryField = value;
            }
        }

        /// <remarks/>
        public string? name
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
        public string? type
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
        public string? land_area
        {
            get
            {
                return this.land_areaField;
            }
            set
            {
                this.land_areaField = value;
            }
        }

        /// <remarks/>
        public byte admin_level
        {
            get
            {
                return this.admin_levelField;
            }
            set
            {
                this.admin_levelField = value;
            }
        }

        /// <remarks/>
        public string? state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public string? border
        {
            get
            {
                return this.borderField;
            }
            set
            {
                this.borderField = value;
            }
        }

        /// <remarks/>
        public uint id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class geodataAdminareas
    {

        private geodataAdminareasAdmin5? admin5Field;

        /// <remarks/>
        public geodataAdminareasAdmin5? admin5
        {
            get
            {
                return this.admin5Field;
            }
            set
            {
                this.admin5Field = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class geodataAdminareasAdmin5
    {

        private uint osmidField;

        private byte levelField;

        private byte admin_levelField;

        private string? boundaryField;

        private string? land_areaField;

        private string? nameField;

        private string? typeField;

        /// <remarks/>
        public uint osmid
        {
            get
            {
                return this.osmidField;
            }
            set
            {
                this.osmidField = value;
            }
        }

        /// <remarks/>
        public byte level
        {
            get
            {
                return this.levelField;
            }
            set
            {
                this.levelField = value;
            }
        }

        /// <remarks/>
        public byte admin_level
        {
            get
            {
                return this.admin_levelField;
            }
            set
            {
                this.admin_levelField = value;
            }
        }

        /// <remarks/>
        public string? boundary
        {
            get
            {
                return this.boundaryField;
            }
            set
            {
                this.boundaryField = value;
            }
        }

        /// <remarks/>
        public string? land_area
        {
            get
            {
                return this.land_areaField;
            }
            set
            {
                this.land_areaField = value;
            }
        }

        /// <remarks/>
        public string? name
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
        public string? type
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class geodataMajor
    {

        private decimal lattField;

        private decimal longtField;

        private ushort elevationField;

        private string? timezoneField;

        private string? cityField;

        private string? provField;

        private object? regionField;

        private string? stateField;

        private decimal inlattField;

        private decimal inlongtField;

        private decimal distanceField;

        /// <remarks/>
        public decimal latt
        {
            get
            {
                return this.lattField;
            }
            set
            {
                this.lattField = value;
            }
        }

        /// <remarks/>
        public decimal longt
        {
            get
            {
                return this.longtField;
            }
            set
            {
                this.longtField = value;
            }
        }

        /// <remarks/>
        public ushort elevation
        {
            get
            {
                return this.elevationField;
            }
            set
            {
                this.elevationField = value;
            }
        }

        /// <remarks/>
        public string? timezone
        {
            get
            {
                return this.timezoneField;
            }
            set
            {
                this.timezoneField = value;
            }
        }

        /// <remarks/>
        public string? city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public string? prov
        {
            get
            {
                return this.provField;
            }
            set
            {
                this.provField = value;
            }
        }

        /// <remarks/>
        public object? region
        {
            get
            {
                return this.regionField;
            }
            set
            {
                this.regionField = value;
            }
        }

        /// <remarks/>
        public string? state
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public decimal inlatt
        {
            get
            {
                return this.inlattField;
            }
            set
            {
                this.inlattField = value;
            }
        }

        /// <remarks/>
        public decimal inlongt
        {
            get
            {
                return this.inlongtField;
            }
            set
            {
                this.inlongtField = value;
            }
        }

        /// <remarks/>
        public decimal distance
        {
            get
            {
                return this.distanceField;
            }
            set
            {
                this.distanceField = value;
            }
        }
    }
}
