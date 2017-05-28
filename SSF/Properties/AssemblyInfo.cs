using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("nSSF")]
[assembly: AssemblyDescription(@"Software for neutron self-shielding calculations
on cylindrical samples according to several authors

Calculation methods:

- MatSSF method
                Dr. Andrej Trkov
- Chilean-Kennedy Sigmoid method
                Dr. Cornelia Chilian &
                Dr. Greg Kennedy
- 
- GEANT4 (Not included yet)

-------------------------------------------------------------
nSSF was developed by:
                Fulvio Farina Arboccò
work:           Physics Department,
                Simón Bolívar University (Caracas, Venezuela)
email:          fulviofarina@usb.ve
-------------------------------------------------------------
The open-source MatSSF code (FORTRAN) was developed by
                Dr. Andrej Trkov (IAEA)
")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("F. Farina Arboccò")]
[assembly: AssemblyProduct("nSSF: neutron self-shielding correction factors")]
[assembly: AssemblyCopyright("F. Farina Arboccò (June 1st, 2017)")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(true)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8f0a347b-33eb-4876-98cd-10fcf82e38f5")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("0.9")]
[assembly: AssemblyFileVersion("0.9")]
[assembly: NeutralResourcesLanguage("en")]