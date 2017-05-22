using System;
using System.Data;

namespace DB
{
    public interface ISampleRow
    {
        double Alpha { get; set; }
        int BlankID { get; set; }
        double CalcDensity { get; set; }
        string CapsuleName { get; set; }
        int CapsulesID { get; set; }
        string ChannelName { get; set; }
        int ChCapsuleID { get; set; }
        bool Comparator { get; set; }
        string Detector { get; set; }
        bool DirectSolcoi { get; set; }
        double DryNet { get; set; }
        string Elements { get; set; }
        bool ENAA { get; set; }
        double f { get; set; }
        double FC { get; set; }
        double FillHeight { get; set; }
        string GeometryName { get; set; }
        LINAA.GeometryRow GeometryRow { get; set; }
        double Gross1 { get; set; }
        double Gross2 { get; set; }
        double GrossAvg { get; set; }
        double Gthermal { get; set; }
        DateTime InReactor { get; set; }
        string IrradiationCode { get; set; }
        int IrradiationRequestsID { get; set; }
        LINAA.IrradiationRequestsRow IrradiationRequestsRow { get; set; }
        double IrradiationTotalTime { get; set; }
        double MatrixDensity { get; set; }
        int MatrixID { get; set; }
        string MatrixName { get; set; }
        LINAA.MatrixRow MatrixRow { get; set; }
        double MoistureContent { get; set; }
        int MonitorsID { get; set; }
        LINAA.MonitorsRow MonitorsRow { get; set; }
        bool NeedsMeasurements { get; }
        bool NeedsPeaks { get; }
        bool NeedsSSF { get; }
        double Net { get; set; }
        string Order { get; set; }
        DateTime OutReactor { get; set; }
        short Position { get; set; }
        double Radius { get; set; }
        int ReferenceMaterialID { get; set; }
        int SamplesID { get; set; }
        LINAA.SamplesRow SamplesRow { get; set; }
        bool Selected { get; set; }
        int StandardsID { get; set; }
        LINAA.StandardsRow StandardsRow { get; set; }
        DateTime SubSampleCreationDate { get; set; }
        string SubSampleDescription { get; set; }
        string SubSampleName { get; set; }
        int SubSamplesID { get; set; }
        string SubSampleType { get; set; }
        object Tag { get; set; }
        double Tare { get; set; }
        LINAA.UnitRow UnitRow { get; }
        LINAA.VialTypeRow VialTypeRow { get; set; }
        LINAA.VialTypeRow VialTypeRowByChCapsule_SubSamples { get; set; }
        double Vol { get; set; }

        void CalculateDensity(bool caldensity);
        double CalculateMass();
        void Check(DataColumn column, bool calMass, bool calRad, bool calFh, bool calDensity);
        bool CheckGthermal();
        bool CheckUnit();
        double DryMass();
        double FindFillingHeight();
        double FindRadius();
        double FindSurface();
        double FindVolumen();
        LINAA.IPeakAveragesRow[] GetIPeakAveragesRows();
        LINAA.IRequestsAveragesRow[] GetIRequestsAveragesRows();
        LINAA.MatrixRow[] GetMatrixRows();
        LINAA.MeasurementsRow[] GetMeasurementsRows();
        LINAA.PeaksRow[] GetPeaksRows();
        LINAA.UnitRow[] GetUnitRows();
        bool IsAlphaNull();
        bool IsBlankIDNull();
        bool IsCalcDensityNull();
        bool IsCapsuleNameNull();
        bool IsCapsulesIDNull();
        bool IsChannelNameNull();
        bool IsChCapsuleIDNull();
        bool IsComparatorNull();
        bool IsDetectorNull();
        bool IsDirectSolcoiNull();
        bool IsDryNetNull();
        bool IsElementsNull();
        bool IsENAANull();
        bool IsFCNull();
        bool IsFillHeightNull();
        bool IsfNull();
        bool IsGeometryNameNull();
        bool IsGross1Null();
        bool IsGross2Null();
        bool IsGrossAvgNull();
        bool IsGthermalNull();
        bool IsInReactorNull();
        bool IsIrradiationCodeNull();
        bool IsIrradiationRequestsIDNull();
        bool IsIrradiationTotalTimeNull();
        bool IsMatrixDensityNull();
        bool IsMatrixIDNull();
        bool IsMatrixNameNull();
        bool IsMoistureContentNull();
        bool IsMonitorsIDNull();
        bool IsNetNull();
        bool IsOrderNull();
        bool IsOutReactorNull();
        bool IsPositionNull();
        bool IsRadiusNull();
        bool IsReferenceMaterialIDNull();
        bool IsSamplesIDNull();
        bool IsStandardsIDNull();
        bool IsSubSampleCreationDateNull();
        bool IsSubSampleDescriptionNull();
        bool IsSubSampleNameNull();
        bool IsSubSampleTypeNull();
        bool IsTareNull();
        bool IsVolNull();
        bool Override(string alpha, string efe, string Geo, string Gt, bool asSamples);
        void SetAlphaNull();
        void SetBlankIDNull();
        void SetCalcDensityNull();
        void SetCapsuleNameNull();
        void SetCapsulesIDNull();
        void SetChannelNameNull();
        void SetChCapsuleIDNull();
        void SetComparatorNull();
        void SetDetectorNull();
        void SetDetectorPosition(string det, string pos);
        void SetDirectSolcoiNull();
        void SetDryNetNull();
        void SetElementsNull();
        void SetENAANull();
        void SetFCNull();
        void SetFillHeightNull();
        void SetfNull();
        void SetGeometryNameNull();
        void SetGross1Null();
        void SetGross2Null();
        void SetGrossAvgNull();
        void SetGthermalNull();
        void SetInReactorNull();
        void SetIrradiationCodeNull();
        void SetIrradiationRequestsIDNull();
        void SetIrradiationTotalTimeNull();
        void SetMatrixDensityNull();
        void SetMatrixIDNull();
        void SetMatrixNameNull();
        void SetMoistureContentNull();
        void SetMonitorsIDNull();
        void SetNetNull();
        void SetOrderNull();
        void SetOutReactorNull();
        void SetPositionNull();
        void SetRadiusNull();
        void SetReferenceMaterialIDNull();
        void SetSamplesIDNull();
        void SetStandardsIDNull();
        void SetSubSampleCreationDateNull();
        void SetSubSampleDescriptionNull();
        void SetSubSampleNameNull();
        void SetSubSampleTypeNull();
        void SetTareNull();
        void SetVolNull();
        bool ShouldSelectIt(bool CheckForSSF);
    }
}