      PROGRAM MATSSF
C-Title  : MATSSF Program
C-Purpose: Calculate Isotopic w/o composition and self-shielding factors
C-Author : A. Trkov, International Atomic Energy Agency, Vienna, Austria
C-Version: 2005/12 Original code
C-V  06/10 Print potential cross section unconditionally (A. Trkov)
C-V  06/11 Correct Avogadro's number in ISOWGT (A.Trkov)
C-V  07/02 Define mass from density&volume, if not given (A.Trkov)
C-V  07/07 - Allow default input MATSSF_INP.TXT
C-V        - Default output filename changed to MATSSF_LST.TXT
C-V  07/08 Print more digits for weight fractions.
C-V  08/06 - Add optional configuration file to define filenames.
C-V        - Add cross section library.
C-V        - Add spectrum selection.
C-V  08/08 Add cross section library option to calculate flux depression
C-V        factors (activation library in 640-group structure).
C-V  09/01 - Improve algorithm for mean chord length (G. Zerovnik)
C-V        - Read additional input parameters to define irradiation
C-V          channel characteristics.
C-V  09/07 - Fix and improve the calculation of thermal self-shielding.
C-V        - Adjust Bell factor for flat-lying wires.
C-V        - Allow comments in input, identified by "--" in columns 1-2.
C-V  09/11 Reorder MATSSF input for channel dimensions to make it more
C-V        easily understandable.
C-V        WARNING - input is not backward-compatible if ISRC.not.=0.
C-V  12/12 - Print comments also to the log file.
C-V        - Add total number density printout to log file.
C-V  15/01 - Fix printout of mass for heavy smples.
C-V        - Check isotopic abundances sum to 100%.
C-V  16/02 Add Bell factor as input parameter.
C-M
C-M  Manual for Program MATSSF
C-M  =========================
C-M
C-M  The program helps the user to calculate the element and isotope
C-M  number densities and self-shielding factors, given the chemical
C-M  composition of the components, their weight-% fraction in the
C-M  mixture, the mass and dimensions of the sample.
C-M
C-M  Instructions
C-M  ------------
C-M  The program is designed to run interactively. The user is
C-M  expected to respond to command prompts on the terminal and
C-M  enter the required information through the keyboard in the
C-M  order as follows.
C-M    If the user prefers, the input instructions can be written to
C-M  file MATSSF_INP.TXT in the same order as they would be entered 
C-M  interactively. The program searches for the existence of the
C-M  file; if it exists, all input is deverted to this file.
C-M    The program expects two libraries:
C-M      MATSSF.DAT      atomic masses, abundances and self-shielding
C-M                      factors
C-M      MATSSF_XSR.TXT  activation cross sections in 640-group
C-M                      ENDF format.
C-M  The default names can be changed in the configuration file
C-M  MATSSF.CFG, which is updated after each MATSSF run.
C-M    The MATSSF_XSR.TXT library is optional. It is needed if the
C-M  use wishes to include resonance interference effects in the
C-M  calculation of the self-shielding factors in multi-material
C-M  samples containing resonance absorbers.
C-M
C-M  Material component definition:
C-M  Material components are defined by a pair of entries. The first
C-M  entry gives the chemical formula and the second the
C-M  corresponding weight-percent fraction.
C-M  - A character string read from input is parsed to identify
C-M    the element (or isotope) by its chemical symbol. Upper-
C-M    or lower-case characters are accepted. Isotopes are
C-M    distinguished from elements in that they contain their
C-M    respective mass number immediately after the chemical symbol,
C-M    with delimiter "-" or no delimiter at all. The element or
C-M    isotope symbol is followed by its molar number or fraction,
C-M    as seen from the examples below.
C-M  - The component weight-percent fraction in the mixture is read
C-M    from the next record. Blank response implies 100%. A negative
C-M    value implies the last entry and the weight fraction is
C-M    calculated internally to make 100% total.
C-M  Several pairs of records may be read to construct the full
C-M  mixture composition, until a blank is entered for a component
C-M  composition. The sum of all weight fractions is normalised
C-M  to 100%.
C-M
C-M  Sample mass and geometry definition
C-M  In order to calculate the self-shielding factors assuming the
C-M  equivalence theorem, which is well established in reactor
C-M  physics, the sample dimensions must be specified. Cylindrical
C-M  geometry is assumed, which is well suited for samples in the
C-M  form of wires of discs. Without any significant loss of
C-M  accuracy, equivalent diameters can be given for square-cut
C-M  foils such that foil area is conserved. The sample density
C-M  in units [g/cm] of the mixture is calculated from the mass
C-M  and the calculated volume. Input requests are as follows:
C-M  - mass of the sample [mg]
C-M  - diameter of the sample [mm]
C-M  - length (wires) or thickness (foils), [mm]
C-M  If any of the above are missing, a request to enter the
C-M  density is issued. An attempt is made to reconstruct a single
C-M  missing quantity, but if more than one is omitted, the calculated
C-M  self-shielding factors correspond to infinite medium.
C-M
C-M  Neutron source definitions
C-M  The mean chord length depends on the assumptions about the
C-M  neutron source term. The built-in models allow the source to
C-M  be isotropic, or distributed uniformly on a finite cylinder
C-M  enclosing the sample (i.e. the irradiation channel). If the
C-M  sample is a wire oriented radially in the channel, the mean
C-M  chord length is independent of the channel dimensions. However,
C-M  if the sample is a wire (long cylinder) or a foil (short cylinder)
C-M  with cylinder axis coinciding with the channel axis, the height
C-M  and the diameter of the irradiation channel are needed to
C-M  calculate the mean chord length.
C-M
C-M  Output
C-M  ------
C-M  The calculated quantities are printed on screen and on the
C-M  MATSSF_LST.TXT file. The labelling of the printed quantities
C-M  is self explanatory.
C-M    The input and normalised percent-weights are calculated
C-M  for each nuclide/element in the sample. Number densities in
C-M  units "x 1E24 atoms/cm3" are given. If self-shielding
C-M  factor tables are available in the library for the nuclide,
C-M  the self-shielding factors and the corresponding Bondarenko
C-M  dilution cross sections are printed. The type of neutron
C-M  source is controlled by an input flag.
C-M
C-M  File units: 1 - MATSSF.DAT element and isotope library, defined
C-M                  and opened internally,
C-M              2 - MATSSF_LST.TXT output file defined and opened
C-M                  internally on each MATSSF run,
C-M              5 - keyboard input or default filename MATSSF_INP.TXT
C-M              6 - terminal screen output.
C-M
C-M  Examples of valid entries to define the chemical composition
C-M  of a component: "  B 2 O 3"           Boron oxide,
C-M                  "al 2 O 3 "           Aluminium oxide,
C-M                  "SI O 2 "             Silicon oxide,
C-M                  "B-10 .199 B11 .801"  natural boron composition.
C-M
C-M  Methods
C-M  -------
C-M  Thermal self-shielding factors (flux depression factors)
C-M  are calculated according to the method described by De Corte,
C-M  with improvements by M. Blaauw.
C-M
C-M  Resonance self-shielding factors are interpolated from tables
C-M  generated by NJOY in three-group structure. Self-shielding
C-M  factors for the second energy group with boundaries at 0.55 eV
C-M  and 2 MeV are included in the MATSSF.DAT library.
C-M
C-M  Self-shielding factors are tabulated in terms of the
C-M  Bondarenko dilution cross section, which is defined for a
C-M  resonance absorber as the macroscopic potential cross section
C-M  of the surrounding nuclei per absorber atom. The macroscopic
C-M  dilution cross section is given by
C-M
C-M       Zb  = Sum(i) n(i)*Sigp(i) + Zgb
C-M
C-M  where  n(i) are the number densities of the surrounding nuclei
C-M  (i) and Sigp(i) are their potential scattering cross sections.
C-M
C-M  According to the equivalence theory the geometrical self-
C-M  shielding is equivalent to material self-shielding through
C-M  a suitably defined geometrical component contribution Zgb,
C-M  which is given by
C-M
C-M              a*
C-M       Zgb = ----
C-M              l
C-M
C-M  where (l) is the mean chord length and (a*) is the Bell factor.
C-M  The nominal value 1.16 is used for the Bell factor for isotropic
C-M  sources and for wires lying along irradiation channel axis.
C-M  If an "isotropic" source is chosen, the Bell factor can be
C-M  redefined from input as appropriate.
C-M
C-M  For wires lying flat in the channel the Bell factor depends on
C-M  the scattering properties of the sample. Scattering causes
C-M  neutrons to be deflected in the direction of the longer dimension,
C-M  thus increasing the effective flux and reducing the self-shielding.
C-M  The expression is:
C-M
C-M         a* = 1.25 + 0.5* Zs/Zt
C-M  where Zs and Zt are the scattering and total cross sections of
C-M  the sample.
C-M
C-M  In its simplest form for an isotropic source, the mean chord
C-M  length is proportional to the volume-to-surface ratio defined
C-M  by the diameter "d" and foil thickness or wire length "h".
C-M
C-M              4V        d h
C-M         l = ----  = ---------
C-M              S       d/2 + h
C-M
C-M  If the source is a cylindrical tube of height H and diameter D,
C-M  the mean chord length can be derived analytically for a wire or
C-M  a foil (i.e. a very short cylinder), lying along the axis of the
C-M  source:
C-M
C-M             Pi h d/2 sqrt( 1 + (H/D)**2 ) arctg(H/D)
C-M         l = --------------------------------------------
C-M             2 d H/D + Pi(d/2) (sqrt( 1 + (H/D)**2 ) - 1)
C-M
C-M  If the wire is lying flat in the channel (perpendicular to channel
C-M  axis), the mean chord length is independent of channel dimensions
C-M  and is given by
C-M
C-M                 Pi^2 r^2 d arctan( H/D )
C-M         l = ----------------------------------------------------
C-M               (                              Pi r^2 H         )
C-M             2 < 2 r d g( arctan(H/D)) + --------------------- >
C-M               (                         D sqrt( 1 + (H/D)^2 ) )
C-M
C-M  where g is a non-elementary function
C-M
C-M         g = Int[0, Pi/2] dp Int[0,x] 
C-M             sqrt( cos^2 p cos^2 q + sin^2 q) dq
C-M
C-M  and can be approximated by an 8-th order polynomial.
C-M  See function GH2R for details.
C-M
C-M  The microscopic Bondarenko dilution cross section is given 
C-M  by the relation
C-M
C-M       Sigb = Zb / n(a)
C-M
C-M  where n(a) is the number density of the absorber nuclei.
C-M
C-M  Resonance interference is taken into account approximately by
C-M  solving the integral slowing-down equation with cross sections
C-M  in 640-group sturcture.
C-M
C-M  Notes:
C-M  -  The code is an extension of the MATCMP code by
C-M     A. Trkov, Institute J.Stefan, Ljubljana, Slovenia (1992)
C-M  -  For details on the origin of the data in the MATSSF.DAT
C-M     library see comments in the file header.
C-M  -  Full details on the methods for the calculation of the
C-M     self-shielding factors can be found in:
C-M
C-M     A. Trkov, G. Zerovnik, L. Snoj, M. Ravnik,
C-M     On the Self-Shielding Factors in Neutron Activation Analysis
C-M     Nuclear Inst. and Methods in Physics Research, A,
C-M     to be published (2009).
C-M
C-
C* Array size limits: Max.No. of isotopes           = MXI
C*                    Max.No. of input constituents = MXI
C*                    No.of elements                = MXE
C*                    MXX                           = MXE*MXI
C*                    Max. No. of self-sh. factors  = MXS
C*                    Max. No. of energy-groups     = MXG
      PARAMETER    (MXS=10, MXI=50 ,MXE=100 ,MXX=5000, MXG=1000)
      PARAMETER    (MR =80)
      LOGICAL      LMAKS
      LOGICAL      EXST
      CHARACTER*80 REC(MXI),LNI
      CHARACTER*80 FLNM,FLIN,FLOU,FLIB,FLXS,FLSP,FLCF
      CHARACTER*66 CH66
      CHARACTER*40 BLN,FL40,CHSRC(3)
      CHARACTER*7  HSSF,HSRI,HSAL
      CHARACTER*2  IS(MXE) ,JS(MXI), KS
      DIMENSION    JZ(MXI) ,JA(MXI), NI(MXE),IA(MXI,MXE),NX(MXI,MXE)
     &            ,AF(MXI) ,FR(MXI), WE(MXI)
     &            ,WT(MXI,MXE),DN(MXI,MXE)
     &            ,AM(MXI,MXE),AB(MXI,MXE)
     &            ,SP(MXI,MXE),GC(MXI,MXE)
     &            ,SSE(MXS,MXI,MXE),SSF(MXS,MXI,MXE)
     &            , FL(MXG),FLR(MXG),FLM(MXG)
     &            ,ENM(MXG),XMA(MXG),XME(MXG)
     &            ,ENR(MXG),XRA(MXG),XRE(MXG)
     &            ,NBT(20),INR(20)
      DATA NI /MXE*0/, WT,DN/MXX*0.,MXX*0./, IS,JS/MXE*'  ',MXI*'  '/
      DATA BLN/'                                        '/
      DATA FLIB/'MATSSF.DAT'/
     &     FLCF/'MATSSF.CFG'/
     &     FLIN/'MATSSF_INP.TXT'/
     &     FLOU/'MATSSF_LST.TXT'/
     &     FLXS/'MATSSF_XSR.TXT'/
     &     FLSP/'MATSSF_SPE.TXT'/
      DATA LIB,LIN,LTT,LOU,LXS,LSP,LCF
     &    /  1,  5,  6,  8, 11, 12, 13/
      DATA PI /3.1415926/
      DATA CHSRC/' Isotropic                              '
     &          ,' Wire flat in a channel                 '
     &          ,' Foil or wire along channel axis        '/
C* Default irradiation channel diameter and height
      CHD =24
      CHT =300
      SRC =0
C* Write banner
      WRITE(LTT,611)
      WRITE(LTT,611) ' MATSSF - Element & isotope composition '
      WRITE(LTT,611) ' -------------------------------------- '
C* Check for the configuration file - read filenames
      INQUIRE(FILE=FLCF,EXIST=EXST)
      IF(EXST) THEN
        OPEN (UNIT=LCF, FILE=FLCF,STATUS='OLD')
        READ (LCF,612,END=8) FLIB
        READ (LCF,612,END=8) FLXS
        READ (LCF,612,END=8) FLNM
C...    READ (LCF,612,END=8) FLSP
    8   CLOSE(UNIT=LCF)
      ELSE
C* Update the configuration file
        OPEN (UNIT=LCF, FILE=FLCF,STATUS='NEW')
        WRITE(LCF,612) FLIB
        WRITE(LCF,612) FLXS
C...    WRITE(LCF,612) FLSP
        CLOSE(UNIT=LCF)
      END IF
C* Check if the cross section library exists
      INQUIRE(FILE=FLXS,EXIST=EXST)
      IF(.NOT.EXST) THEN
        CALL CHLEN(FLXS,LN,80)
        L1=LN-40+1
        IF(L1.LT.1) THEN
          L1=1
          FL40=FLXS(L1:LN)
        ELSE
          FL40=FLXS(L1:LN)
          FL40(1:3)='...'
        END IF
        WRITE(LTT,611)
        WRITE(LTT,611) ' WARNING - Non-existent x.s. file     : ',FL40
        WRITE(LTT,611) '    No resonance interference calculated'
        WRITE(LTT,611)
        LXS=-ABS(LXS)
      ELSE
        OPEN (UNIT=LXS, FILE=FLXS,STATUS='OLD')
      END IF
C* Check for default input file
      INQUIRE(FILE=FLIN,EXIST=EXST)
      IF(EXST) THEN
        CALL CHLEN(FLIN,LN,80)
        L1=LN-40+1
        IF(L1.LT.1) THEN
          L1=1
          FL40=FLIN(L1:LN)
        ELSE
          FL40=FLIN(L1:LN)
          FL40(1:3)='...'
        END IF
        WRITE(LTT,611)
        WRITE(LTT,611) ' Input read from default input file   : ',FL40
        WRITE(LTT,611)
        OPEN (UNIT=LIN, FILE=FLIN,STATUS='OLD')
      END IF
C* Open the isotopic mass and abundance file
      OPEN (UNIT=LIB,FILE=FLIB,STATUS='OLD',ERR=92)
C* Read atomic number, symbol, mass number, atomic weight, abundance,
C* potential scattering cross section, Goldstein-Cohen lambda, No.of SSF
   10 READ (LIB,191,ERR=10,END=12) KZ,KS,KA,XM,XB,XP,GCL,KX
      IF(KZ .LE.0 .OR. KZ.GT.MXE) GO TO 10
      IF(KA .EQ.0) XB = 100
      IF(GCL.LE.0) GCL= 1
      IF(NI(KZ).GE.MXI) THEN
        PRINT *,'WARNING - Isotope table truncated',KZ,KS,KA
        GO TO 10
      END IF
      IS(KZ)  = KS
      NI(KZ)  = NI(KZ)+1
      K       = NI(KZ)
      AM(K,KZ)= XM
      AB(K,KZ)= XB
      IA(K,KZ)= KA
      SP(K,KZ)= XP*GCL
      GC(K,KZ)= GCL
      NX(K,KZ)= KX
      IF(KX.GT.0) THEN
        IF(KX.GT.MXS) STOP 'MATSSF ERROR - MXS Limit exceeded'
        READ (LIB,192) (SSE(KX+1-J,K,KZ),SSF(KX+1-J,K,KZ),J=1,KX)
      END IF
      GO TO 10
   12 CLOSE(UNIT=LIB)
C* Check that isotopic abundances sum up to 100%
      CALL CHKABN(MXE,MXI,IS,NI,AB)
C*
C* Mass and self-shielding library read - start reading input data
   18 IC  = 0
      TFR = 0
      DNS = 1
      VOL = 0
      ZGB = 0
      CDL = 0
C*
C* Bell Factors     
      BEL0= 1.16
      BEL1= 1.30
      BEL2= 1.16
C*
      GTH = 1
      DO I=1,MXE
        DO J=1,MXI
          WT(J,I)=0
          DN(J,I)=0
        END DO
      END DO
      WRITE(LTT,611)
      WRITE(LTT,611) ' Enter components terminated by blank   '
C* Process a component composition input record (MR char.max.)
   20 WRITE(LTT,611) '$      Component chemical composition : '
   21 READ (LIN,612) LNI
      IF(LNI(1:2).EQ.'--') THEN
        WRITE(LTT,612) LNI
        WRITE(LOU,612) LNI
        GO TO 21
      END IF
      IF(LNI(1:40).EQ.BLN) GO TO 60
      NE = 0
      IR = 1
      IC = IC+1
      REC(IC)=LNI
C* Decode the input record from character I to MR
   22 NE = NE+1
      CALL MATINP(REC(IC),IR,MR,JS(NE),JA(NE),AF(NE),IER)
      IF(IER.EQ.0) GO TO 22
      NE = NE-1
C* Input the component fraction in the material 
   24 WRITE(LTT,611) '$Enter component fraction       [w/o] : '
   25 READ (LIN,612) LNI
      IF(LNI(1:2).EQ.'--') THEN
        WRITE(LTT,612) LNI
        WRITE(LOU,612) LNI
        GO TO 25
      END IF
      FRI=100.
      IF(LNI(1:40).NE.BLN) READ (LNI,614,ERR=24) FRI
      WRITE(LTT,612)
C* Component elements/isotopes successfully identified
      IF(FRI.LT.0) THEN
        FR(IC)= 100-TFR
        TFR=100
      ELSE
        FR(IC)=FRI
        TFR= TFR+FRI
      END IF
C* Find atomic number indices and weights for all constituents
      WC = 0.
      DO I=1,NE
        KZ = NUMATM(MXE,IS,JS(I))
        IF(KZ.LE.0) THEN
C* Error condition - a component element/isotope not in the library
          WRITE(LTT,621) JS(I),JA(I)
          IC = IC-1
          GO TO 20
        END IF
        WJ = ELMWGT(JA(I),NI(KZ),IA(1,KZ),AM(1,KZ))
        IF(WJ.LE.0) THEN
C* Error condition - a component element/isotope not in the library
          WRITE(LTT,621) JS(I),JA(I)
          IC = IC-1
          GO TO 20
        END IF
        WC = WC + AF(I)*WJ
        WE(I)= WJ
        JZ(I)= KZ
      END DO
C* Determine the weight fractions of the constituents
      DO I=1,NE
        WE(I)=FR(IC)*AF(I)*WE(I)/WC
      END DO
C* Loop over component elements to determine isotope weights [%]
      DO I=1,NE
        KZ=JZ(I)
        CALL ISOWGT(JA(I),NI(KZ),IA(1,KZ),AM(1,KZ),AB(1,KZ)
     &                          ,WT(1,KZ),DN(1,KZ),WE(I))
      END DO
      IF(IC.LT.MXI .AND. FRI.GE.0) GO TO 20
      WRITE(LTT,611)
      WRITE(LTT,611) ' Enter sample geometry data and mass    '
      WRITE(LTT,611)
      GO TO 60
   58 WRITE(LTT,611) ' ERROR entering sample data - REDO      '
C*
C* Enter the sample dimensions
   60 IF(IC.LT.1) GO TO 90
      WRITE(LTT,611) '$Enter sample mass               [mg] : '
   61 READ (LIN,612) LNI
      IF(LNI(1:2).EQ.'--') THEN
        WRITE(LTT,612) LNI
        WRITE(LOU,612) LNI
        GO TO 61
      END IF
      READ (LNI,614,ERR=58) SMM
      SMM=SMM/1000
      WRITE(LTT,611) '$Enter sample diameter           [mm] : '
   62 READ (LIN,612) LNI
      IF(LNI(1:2).EQ.'--') THEN
        WRITE(LTT,612) LNI
        WRITE(LOU,612) LNI
        GO TO 62
      END IF
      READ (LNI,614,ERR=58) SMD
      SMD=SMD/10
      WRITE(LTT,611) '$Enter sample length/thickness   [mm] : '
   63 READ (LIN,612) LNI
      IF(LNI(1:2).EQ.'--') THEN
        WRITE(LTT,612) LNI
        WRITE(LOU,612) LNI
        GO TO 63
      END IF
      READ (LNI,614,ERR=58) SMT
      SMT=SMT/10
      IF(SMT.GT.0 .AND. SMD.GT.0) THEN
        VOL=SMT*SMD*SMD*PI/4
        IF(SMM.GT.0) THEN
          DNS=SMM/VOL
          GO TO 68
        END IF
      END IF
      WRITE(LTT,611) '$Enter density of the mixture [g/cm3] : '
   64 READ (LIN,612) LNI
      IF(LNI(1:2).EQ.'--') THEN
        WRITE(LTT,612) LNI
        WRITE(LOU,612) LNI
        GO TO 64
      END IF
      READ (LNI,614,END=68,ERR=60) DNS
      IF(DNS.GT.0) THEN
        IF(SMM.GT.0) THEN
          VOL=SMM/DNS
          IF(SMD.LE.0 .AND. SMT.GT.0) THEN
            SMD=2*SQRT(VOL/SMT/PI )
          ELSE IF(SMT.LE.0 .AND. SMD.GT.0) THEN
            SMT=VOL/(PI*SMD*SMD/4)
          ELSE
            WRITE(LTT,611) ' WARNING - sample data incomplete       '
            WRITE(LTT,611) '           Self-shielding at inf.dil.   '
          END IF
        ELSE
          SMM=DNS*VOL
        END IF
      END IF
C*
   68 ISRC=0
      IF(SMT.GT.0 .AND. SMD.GT.0) THEN
        WRITE(LTT,611) ' Neutron source definitions:            '
        WRITE(LTT,611) ' ISRC  Description                      '
        WRITE(LTT,611) '   0 = Isotropic                        '
        WRITE(LTT,611) '   1 = Wire flat in a channel           '
        WRITE(LTT,611) '   2 = Foil or wire along channel axis: '
        WRITE(LTT,611) ' Default irradiation channel data       '
        WRITE(LTT,611) ' CHD   Irradiation channel diameter [mm]'
        WRITE(LTT,611) ' CHT   Channel active height [mm]       '
        WRITE(LTT,611) '                                        '
        WRITE(LTT,611) '$Enter ISRC (0, 1, or 2)              : '
   69   READ (LIN,612,END=70) LNI
        IF(LNI(1:40).NE.BLN) READ (LNI, * ,ERR=70,END=70) SRC
        ISRC=NINT(SRC)
        IF((ISRC.EQ.1).OR.(ISRC.EQ.2)) THEN
          WRITE(LTT,611) ' Irradiation channel data               '
          WRITE(LTT,616) '           Default irrad.ch.diam.[cm] : ',CHD
          WRITE(LTT,611) '$          Enter new value to redefine: '
          READ(LIN,612,END=70) LNI
          IF (LNI(1:40).NE.BLN) READ(LNI,*) CHD
          WRITE(LTT,616) '           Default irrad.ch.length[cm]: ',CHT
          WRITE(LTT,611) '$          Enter new value to redefine: '
          READ(LIN,612,END=70) LNI
          IF (LNI(1:40).NE.BLN) READ(LNI,*) CHT
C*
          IF(ISRC.EQ.1) THEN
C*          --Mean chord length for a wire lying flat
C*            in a cylindrical source
            H2R=CHT/CHD
            AH2R=ATAN(H2R)
            CDL=(PI*SMD/2)**2 * SMT *AH2R /
     &          (2* (SMD*SMT*GH2R(AH2R) +
     &           PI*SMD*SMD*CHT/(4*CHD*SQRT(1+H2R*H2R))))
C           CDL=PI*SMD*SMT/(2*PI*SMD+8*SMT))
          ELSE
C*          -- Mean chord length for a cylindrical source of 
C*             diameter CHD, height CHT
            CDL=(PI*SMD*SMT/2)*SQRT(1+(CHT/CHD)**2)*ATAN(CHT/CHD)
     &         /((SMT*CHT*2/CHD) + (PI*SMD/2)*(SQRT(1+(CHT/CHD)**2)-1))
          END IF
        ELSE
C*        -- Isotropic source
C*           Redefine the Bell factor, if necessary
          WRITE(LTT,616) ' Default Bell factor                  : ',BEL0
          WRITE(LTT,611) '$         Enter new value to redefine : '
          READ (LIN,612,END=70) LNI
          IF(LNI(1:40).NE.BLN) THEN
            READ (LNI, *) BEL0
            BEL =BEL0
          END IF
C*        -- Mean chord length for isotropic source = 4V/S =dh/(d/2+h)
          CDL=SMD*SMT/(SMD/2+SMT)
        END IF
C*
      END IF
C*
C* Begin calculations
   70 IF(DNS.LE.0) DNS=1
      RD =100./TFR
      RR =DNS*RD
      GCP=0
      SGP=0
      AMS=0
      DO I=1,MXE
        NK=NI(I)
        DO J=1,NK
C* Normalise number density to actual material density
          WTJ    =WT(J,I)
          DNJ    =DN(J,I)*RR
          DN(J,I)=DNJ
c...
c...          if(wtj.gt.0) print *,'i,j,w,d,sp',i,j,wtj,dnj,nx(j,i),sp(j,i)
c...
          IF(DNJ.GT.0) THEN
C* Sum nuclide contribution to potential cross section of mixture
            IF(J.GT.1 .AND. SP(1,I).LE.0) THEN
              GCP=GCP+DNJ*SP(J,I)/GC(J,I)
              SGP=SGP+DNJ*SP(J,I)
              AMS=AMS+DNJ*AM(J,I)
            END IF
c...
c...            print *,'DNi,SPi,SGP',DNJ,SP(J,I),SGP
c...
          END IF
        END DO
      END DO
      NG  =0
      DO J=1,MXG
        XMA(J)=0
        XME(J)=0
      END DO
      DNTOT=0
      SGA0 =0
      SGE0 =0
C*
C* Sum contributions to the absorption and scattering of the mixture
C* Total=MT1, absorption=MT102, scattering=difference
      IF(LXS.GT.0) THEN
        WRITE(LTT,611) BLN
        WRITE(LTT,611) ' Scanning the cross section library ... '
        DO I=1,MXE
          REWIND LXS
          CALL RDTEXT(LXS,MAT,MF,MT,CH66,IER)
          NK =NI(I)
          IDN=0
          DO J=1,NK
            WTJ    =WT(J,I)
            IF(WTJ.LE.0) CYCLE
c...
c...        PRINT *,'Looking for',i,ia(j,i),wtj,dn(j,i),IDN
c...
            IF(IDN.EQ.0) DNTOT=DNTOT+DN(J,I)
            IDN  =1
            IMT1 =0
            IZA  =I*1000+IA(J,I)
   72       CALL RDTEXT(LXS,MAT,MF,MT,CH66,IER)
            IF(IER.NE.0 .OR. MAT.LT.0) THEN
c...
c...          PRINT *,'Could not find ',IZA,' MAT=',MAT,' IER=',IER
c...
              REWIND LXS
              CYCLE
            END IF
            IF( MF.NE.3) GO TO 72
            IF( MT.EQ.0) GO TO 72
            READ (CH66,'(F11.0)') C1
            JZA=NINT(C1)
            IF(JZA.NE.IZA) THEN
              DO WHILE (MAT.NE.0)
                CALL RDTEXT(LXS,MAT,MF,MT,CH66,IER)
              END DO
              GO TO 72
            END IF
C* Locate total or capture data
            IF     (MT.EQ.  1) THEN
C*            Read the total cross sections
              CALL RDTAB1(LXS,C1,C2,L1,L2,N1,N2,NBT,INR,ENR,XRE,MXG,IER)
              IF(IER.NE.0) THEN
                STOP 'MATSSF ERROR - reading cross section library'
              END IF
              IMT1=1
            ELSE IF(MT.EQ.101 .OR. MT.EQ.102) THEN
C*            Read the absorption cross sections
              CALL RDTAB1(LXS,C1,C2,L1,L2,N1,N2,NBT,INR,ENR,XRA,MXG,IER)
              IF(IER.NE.0) THEN
                STOP 'MATSSF ERROR - reading cross section library'
              END IF
            ELSE
c...
c...              print *,'skip',mat,jza,mt
c...
              DO WHILE (MT.NE.0)
                CALL RDTEXT(LXS,MAT,MF,MT,CH66,IER)
              END DO
              GO TO 72
            END IF
C* Check the energy grid for consistency
            IF(NG.EQ.0) THEN
              NG=N2
              DO K=1,NG
                ENM(K)=ENR(K)
              END DO
            END IF
            IF(MT.EQ.1)  GO TO 72
c...
c...        PRINT *,'Found IZA,JZA,MAT,MT,IMT1',IZA,JZA,MAT,MT,IMT1
c...
            IF(N2.NE.NG) THEN
              STOP 'MATSSF ERROR - Inconsistent group structure'
            END IF
C* Define absorption and scattering cross sections
            XE=0
c...
c...        write(lou,*) 'first read'
c...
            DO K=1,NG
              IF(ENM(K).NE.ENR(K)) STOP 'MATSSF ERROR - Energy grid'
              IF(IMT1.EQ.1) XE=XRE(K)-XRA(K)
              XA=XRA(K)
c...
c...              if(ENM(k).ge.0.55 .and. ENM(k+1).le.1.) then
c...                write(lou,*) k,ENM(k),xa,xe*gc(j,i),dn(j,i)
c...              end if
c...
              XRA(K)=XA
              XRE(K)=XE
C*            -- Absorption and scattering x.s. at E0
              E0=0.0253
              IF(ENM(K).LT.E0 .AND. ENM(K+1).GT.E0) THEN
                SGE0=SGE0+DN(J,I)*XE
                SGA0=SGA0+DN(J,I)*XA
     &              *(ENM(K+1)-ENM(K))/ALOG(ENM(K+1)/ENM(K)) /E0
              END IF
            END DO
C* Add contribution to the absorption and elastic
            DO K=1,NG
              XMA(K)=XMA(K)+XRA(K)*DN(J,I)
c...          XME(K)=XME(K)+XRE(K)*DN(J,I)
C*            -- Scaling elastic by the Goldstein-Cohen parameter 
C*               empirically gives better results
              XME(K)=XME(K)+XRE(K)*DN(J,I)*GC(J,I)
c...
c...        if(ENM(k).lt.0.0253 .and. ENM(k+1).gt.0.0253)
c... &      PRINT *,'Doing IZA,MAT,MT,XS',IZA,JZA,MAT,MT,XMA(K),DN(J,I)
c...
            END DO
C*          If elemental data found, do not search isotopic
            IF(IA(J,I).EQ.0) EXIT
          END DO
        END DO
        DO J=2,NG
C* Define the spectrum (pure 1/E)
          FL(J-1)=ALOG(ENM(J)/ENM(J-1))
        END DO
C* Absorption and elastic resonance integral
        RSGA=0
        RSGE=0
        DO J=2,NG
          IF(ENM(J).GT.0.55 .AND. ENM(J-1).LT.2.0E6) THEN
            RSGA=RSGA+XMA(J-1)*FL(J-1)
            RSGE=RSGE+XME(J-1)*FL(J-1)
          END IF
        END DO
        IF(ISRC.EQ.1) THEN
C*        -- Tune the Bell factor depending on the ratio of
c*           elastic/total
          BEL=BEL1+0.5*(RSGE/(RSGA+RSGE))
          print *,' *** Bell factor redefined',bel,bel1,rsge,rsga
        ELSE IF(ISRC.EQ.2) THEN
          BEL=BEL2
        ELSE
          BEL=BEL0
        END IF
C* Geometric contribution to dilution cross section
        IF(CDL.GT.0) ZGB=BEL/CDL
        DO J=1,NG
C*        -- Normalise summed macroscopic x.s. with number density
          XMA(J)=XMA(J)/DNTOT
          XME(J)=XME(J)/DNTOT
        END DO
C-F Calculate the thermal self-shielding factor
        GTH=1
        IF(CDL.GT.0) THEN
C* Average atomic weight
          AMS=AMS/DNTOT
C* Average Goldstein-Cohen parameter
          GCP=SGP/GCP
C* Average microscopic potential cross section of the matrix
          SP0=SGP/DNTOT
C* Scale 1/v MACROSCOPIC absorption cross section by sqrt(Pi)/2
C* to get the average cross section. Elastic is not affected.
C         SGA0=SGA0*(SQRT(3.1415926)/2)
C* Note: k0-IAEA manual states that it should be DIVIDED by sqrt(Pi)/2
          SGA0=SGA0/(SQRT(3.1415926)/2)
C* Total thermal macroscopic cross section
          SGT0=SGA0+SGE0
c...
          print *,'zgT0,Zga0,Zge0',sgt0,sga0,sge0
c...
C* Use absorption to calculate Xi (De Corte, thesis)
c***      XI  =SGA0*CDL/2
C* Use total to calculate Xi (Blaauw, NSE 124, 431-435 (1996))
          XI  =SGT0*CDL/2
C* Thermal self-shielding factor for slabs, wires, spheres
          GSL0=GTSLAB(XI)
          GWI0=1-4*XI/3
          GSF0=GTSPHERE(XI)
C* Correction according to Blaauw, NSE 124, 431-435 (1996)
C* Although the correction is derived strictly for slabs, it is
C* applied to spheres and wires as well.
C* WGT is an ad-hoc weight that reduces the correction,
C*     depending on the source geometry (anisotropy effects).
          IF(ISRC.EQ.0) THEN
            WGT=1
          ELSE IF(ISRC.EQ.1) THEN
            WGT=0.93
          ELSE
            WGT=0.67
          END IF
          GSL=GSL0/(1 - WGT*(1-GSL0)*SGE0/(SGT0) )
          GWI=GWI0/(1 - WGT*(1-GWI0)*SGE0/(SGT0) )
          GSF=GSF0/(1 - WGT*(1-GSF0)*SGE0/(SGT0) )
C*        -- Special formula for discs if XI is small
C...         This is no longer applicable with Blaauw corrections
C...      IF(XI.LT.0.3)
C... &      GTH =GSL + (8*XI**(1.4)*EXP(-3.72*XI)
C... &                             +0.4*EXP(8*XI))*(GSF-GSL)
C*        -- Approximate foil, wire or sphere
          IF     (  SMD.GT.SMT) THEN
C*          -- Diameter>>thickness - approximate sphere-->foil
            WW=(SMD-SMT)/SMD
            GTH=GSF*(1-WW) + GSL*WW
          ELSE
C*          -- Diameter<<thickness - approximate sphere-->wire
            WW=(SMT-SMD)/SMT
            GTH=GSF*(1-WW) + GWI*WW
          END IF
c...
          PRINT *,'XI,GTH,GCP,WGT',XI,GTH,GCP,WGT
          PRINT *,'   GLS',GSL0,GSL
          PRINT *,'   GWI',GWI0,GWI
          PRINT *,'   GSF',GSF0,GSF
c...
        END IF
      END IF
C*
C* Print the isotopic composition
      OPEN (UNIT=LOU,FILE=FLOU,STATUS='UNKNOWN')
      WRITE(LTT,691)
      WRITE(LOU,691)
      DO I=1,IC
        WRITE(LTT,692) REC(I),FR(I)
        WRITE(LOU,692) REC(I),FR(I)
      END DO
      WRITE(LTT,693) TFR
      WRITE(LTT,611) BLN
      WRITE(LTT,611) ' Mass, abundance & SSF library        : ',FLIB
      IF(LXS.GT.0)
     &WRITE(LTT,611) ' Cross sections library               : ',FLXS
      WRITE(LTT,611) BLN
      WRITE(LTT,695) ' Material density ',DNS,' [g/cm3]'
      WRITE(LOU,693) TFR
      WRITE(LOU,611) BLN
      WRITE(LOU,611) ' Mass, abundance & SSF library        : ',FLIB
      WRITE(LOU,611) ' Cross sections library               : ',FLXS
      WRITE(LOU,611) BLN
      WRITE(LOU,695) ' Material density ',DNS,' [g/cm3]'
      IF(VOL.GT.0) THEN
C*      Convert dimensions to mm or mg where convenient
        IF(SMM.LT.100) THEN
        WRITE(LTT,695) ' Mass             ',SMM*1000,' [mg]   '
        ELSE
        WRITE(LTT,695) ' Mass             ',SMM,     ' [g]    '
        END IF
        WRITE(LTT,695) ' Diameter         ',SMD*10  ,' [mm]   '
        WRITE(LTT,695) ' Length/thickness ',SMT*10  ,' [mm]   '
        WRITE(LTT,695) ' Mean chord length',CDL     ,' [cm]   '
        WRITE(LTT,695) ' Escape x.sect.   ',ZGB     ,' [1/cm] '
        WRITE(LTT,695) ' Potential x.sect.',SGP     ,' [1/cm] '
        WRITE(LTT,695) BLN
        WRITE(LTT,611) ' Neutron source                         '
     &                ,CHSRC(ISRC+1)
        IF(ISRC.GT.1) THEN
          WRITE(LTT,695) ' Channel diameter ',CHD*10  ,' [mm]   '
          WRITE(LTT,695) ' Channel height   ',CHT*10  ,' [mm]   '
        END IF
        WRITE(LTT,695) BLN
        WRITE(LTT,695) ' G-thermal        ',GTH
        WRITE(LTT,695) BLN
C*
        IF(SMM.LT.100) THEN
        WRITE(LOU,695) ' Mass             ',SMM*1000,' [mg]   '
        ELSE
        WRITE(LOU,695) ' Mass             ',SMM,     ' [g]    '
        END IF
        WRITE(LOU,695) ' Diameter         ',SMD*10  ,' [mm]   '
        WRITE(LOU,695) ' Length/thickness ',SMT*10  ,' [mm]   '
        WRITE(LOU,695) ' Mean chord length',CDL     ,' [cm]   '
        WRITE(LOU,695) ' Escape x.sect.   ',ZGB     ,' [1/cm] '
        WRITE(LOU,695) ' Potential x.sect.',SGP     ,' [1/cm] '
        WRITE(LOU,695) BLN
        WRITE(LOU,611) ' Neutron source                         '
     &                ,CHSRC(ISRC+1)
        IF(ISRC.GT.1) THEN
          WRITE(LOU,695) ' Channel diameter ',CHD*10  ,' [mm]   '
          WRITE(LOU,695) ' Channel height   ',CHT*10  ,' [mm]   '
        END IF
        WRITE(LOU,695) BLN
        WRITE(LOU,695) ' G-thermal        ',GTH
        WRITE(LTT,695) BLN
      END IF
C*
C* Perturbed flux due to the matrix material
C*    -- Average parameters of the matrix nuclides
c     S0 =ZGB/DNJ
      S0 =ZGB/DNTOT
      SP0=SGP/DNTOT
      CALL FLXSLD(NG,AMS,SP0,S0,ENM,FL,XMA,XME,FLM)
c...
c...  print *,'Nuclide ng,ams,s0',ng,ams,s0
c...
c...      open (unit=40,file='flx_gz.cur',status='unknown')
c...c...      open (unit=40,file='flx_ir.cur',status='unknown')
c...      write(40,*) ' Flux Slowing-Down approximation'
c...      do k=2,ng
c...        write(40,'(1p,2e11.4)') sqrt(enm(k)*enm(k-1))
c...     &                         ,flm(k-1)/alog(enm(k)/enm(k-1))
c...      end do
c...      write(40,*) ' '
c...
C* Define the dilution cross section
      SGP0=SGP+ZGB
C* Skip line and write header to output and screen
      WRITE(LTT,611)
      WRITE(LTT,694)
      WRITE(LOU,611)
      WRITE(LOU,694)
C* Write to output all nuclides present
      WTJT=0
      WTRT=0
      DNJT=0
      IELM=0
      DO I=1,MXE
        NK=NI(I)
        IP = 0
        DO J=1,NK
          WTJ    =WT(J,I)
          DNJ    =DN(J,I)
          KX     =NX(J,I)
          IF(WTJ.GT.0) THEN
C*          Bondarenko background cross section
            SGJ=SGP0/DNJ-SP(J,I)
C*          -- Calculate resonance interference from cross sections
            HSRI='        '
            HSAL='        '
            FF21=1
            IF(LXS.GT.0) THEN
C*            Find the nuclide in the cross section library
              REWIND LXS
              CALL RDTEXT(LXS,MAT,MF,MT,CH66,IER)
              IMT1=0
              IZA =I*1000+IA(J,I)
   82         CALL RDTEXT(LXS,MAT,MF,MT,CH66,IER)
              IF(IER.NE.0 .OR. MAT.LT.0) GO TO 84
              IF( MF.NE.3) GO TO 82
              IF( MT.EQ.0) GO TO 82
              READ (CH66,'(F11.0)') C1
              JZA=NINT(C1)
              IF(JZA.NE.IZA) THEN
                DO WHILE (MAT.NE.0)
                  CALL RDTEXT(LXS,MAT,MF,MT,CH66,IER)
                END DO
                GO TO 82
              END IF
C*            -- Locate total or capture data
              IF     (MT.EQ.  1) THEN
C*              Read the total cross sections
                CALL RDTAB1(LXS,C1,C2,L1,L2,N1,N2,NBT,INR
     &                     ,ENR,XRA,MXG,IER)
                IF(IER.NE.0) THEN
                  STOP 'MATSSF ERROR - reading cross section library'
                END IF
                IMT1=1
              ELSE IF(MT.EQ.101 .OR. MT.EQ.102) THEN
C*              Read the absorption cross sections
                CALL RDTAB1(LXS,C1,C2,L1,L2,N1,N2,NBT,INR
     &                     ,ENR,XRE,MXG,IER)
                IF(IER.NE.0) THEN
                  STOP 'MATSSF ERROR - reading cross section library'
                END IF
c...
c...            PRINT *,'Found MAT,MF,MT',MAT,MF,MT,JZA
c...
              ELSE
                DO WHILE (MT.NE.0)
                  CALL RDTEXT(LXS,MAT,MF,MT,CH66,IER)
                END DO
                GO TO 82
              END IF
              IF(MT.EQ.1)  GO TO 82
C*
C* Define absorption and scattering cross sections
              XE=0
              DO K=1,NG
                IF(IMT1.EQ.1) XE=XRA(K)-XRE(K)
                XA=XRE(K)
                XRA(K)=XA
                XRE(K)=XE*GC(J,I)
              END DO
C* Perturbed flux due to the resonance absorber
              AMJ= AM(J,I)
              SPJ=SP(J,I)
              GCJ=GC(J,I)
              S0J=SGP0/DNJ-SPJ
c...
c...          print *,'Matrix ng,amj,s0j',ng,amj,s0j
c...
              CALL FLXSLD(NG,AMJ,SPJ,S0J,ENR,FL,XRA,XRE,FLR)
C* Resonance interference correction
              RLX=0
              RLA=0
              R1X=0
              FLX=0
              FLA=0
              F1X=0
c...
c...          print *,'ng',ng,IZA
c...          print *,'zgb,sp,dnj,dntot',zgb,sp(j,i),dnj,dntot
c...      write(lou,*)'zgb,sp,dnj,dntot',zgb,sp(j,i),dnj,dntot
c...           IF(iza.eq.28058) write(91,*) 'Tot_g'
c...
              DO K=2,NG
                IF(ENM(K-1).GE. 0.55 .AND. ENM(K).LE.2.0E6) THEN
                  Z2A= XMA(K-1)
                  Z2E= XME(K-1)
                  X1A= XRA(K-1)
                  X1E= XRE(K-1)
                  FF0= FL (K-1)
c...
c...              if(ENM(k-1).ge.0.55 .and. ENM(k).le.1.) then
c...              if(ENM(k-1).ge.1.5e6 .and. ENM(k).le.2.0e6) then
c...                write(lou,*) k,ENM(k-1),z2a,z2e,x1a,x1e
c...              end if
c...
C...
C*                -- Flux for pure absorber
C...              FF1=FF0*(ZGB/DNJ)/(ZGB/DNJ + X1E + X1A)
C...              FF1=FF0*(S0J    )/(S0J     + X1E + X1A)
                  FF1=FLR(K-1)
C*                -- Flux for matrix mixture
C...              FF2=FF0*(ZGB    )/(ZGB     + Z2E + Z2A)
C...
                  FF2=FLM(K-1)
c...
c...              if(ENM(k-1).ge.0.55 .and. ENM(k).le.1.) then
c...              if(ENM(k-1).ge.300. .and. ENM(k).le.400.) then
c...              if(ENM(k-1).ge.1.5e6 .and. ENM(k).le.2.0e6) then
c...                write(lou,*) k,ENM(k-1),ff0,ff1,ff2
c...              end if
c...
C*
C* Reaction rates at inf.dil., absorber only, absorber in matrix
                  RLA=RLA+X1A*FF0
                  R1X=R1X+X1A*FF1
                  RLX=RLX+X1A*FF2
C* Flux integral at inf.dil., absorber only, absorber in matrix
                  FLA=FLA+    FF0
                  F1X=F1X+    FF1
                  FLX=FLX+    FF2
C...
C...              if(ENM(k).lt.1) print *,k,ENM(k),fl(k),XMA(k),XMA(k)*DNJ
C...
                END IF
              END DO
c...
c...              PRINT     *, 'RLA,R1X,RLX,FLA,F1X,FLX'
c... &                         ,RLA,R1X,RLX,FLA,F1X,FLX
C...              WRITE(LOU,*) 'RLA,R1X,RLX,FLA,F1X,FLX'
C... &                         ,RLA,R1X,RLX,FLA,F1X,FLX
c...
              SLA=RLA/FLA
              SLX=RLX/FLX
              S1X=R1X/F1X
c...
              FFRI=(RLX/RLA)
              FF21=(RLX/R1X)
              FFSS=(S1X/SLA)
              WRITE(HSRI,194) FFRI
            END IF
   84       IF(KX.GT.0) THEN
C*            Interpolate self-shielding factors, if available
c...
c...              print *,'sgp,sgj,sg,dnj',sgp0,sgj,sp(j,i),dnj
c...
C* Find Sig0 interval enclosing SGJ and interpolate
              K=0
              DO L=2,KX
                IF(SSE(L-1,J,I).LE.SGJ) K=L
              END DO
C* Check the Sig0 interval
              IF(K.LE.0) THEN
                PRINT *,'WARNING - SSF table extrapolation'
                FFSF=SSF(1,J,I)
              ELSE
C* Interpolate the self-shielding factor
                RI=1
                CALL RESPOL(SSE(K-1,J,I),SSE(K,J,I),SGJ,RI
     &                     ,SSF(K-1,J,I),SSF(K,J,I),FFSF,LMAKS)
              END IF
              WRITE(HSSF,194) FFSF
C...
C...          PRINT *,'SSFp,SSFg,GFg,GF*',ffsf,ffss,ffri,ffsf*ffri/ffss
C...          write(lou,*) 'SSFp,SSFg,GFg,GF*'
C... &                     ,ffsf,ffss,ffri,ffsf*ffri/ffss
C...
C*            -- Total flux depression with self-shielding correction
c...          WRITE(HSAL,194) FFSF*FFRI/FFSS
C*            -- Matrix flux depression * self-shielding
C...          WRITE(HSAL,194) FFSF*FFRI
C*            -- Self-shielded RR corrected for matrix interference
              WRITE(HSAL,194) FFSF*FF21
            ELSE
              HSSF='        '
              HSAL=HSRI
            END IF
            IP = IP+1
            IF(IS(I).NE.KS) THEN
            
C              PRINT *,'KS,IS,I ',KS,IS(I),I

              KS=IS(I)
C              WRITE(LTT,696)
C              WRITE(LOU,696)
            END IF
            JIA=IA(J,I)
            WTR=WTJ*RD
            WRITE(LTT,696) I,IS(I),JIA,WTJ,WTR,DNJ,SGJ,HSSF,HSAL
            WRITE(LOU,696) I,IS(I),JIA,WTJ,WTR,DNJ,SGJ,HSSF,HSAL
            IF(JIA.EQ.0) THEN
              WTJT=WTJT+WTJ
              WTRT=WTRT+WTR
              DNJT=DNJT+DNJ
              IELM=I
            ELSE
              IF(I.NE.IELM) THEN
                WTJT=WTJT+WTJ
                WTRT=WTRT+WTR
                DNJT=DNJT+DNJ
              END IF
            END IF
          END IF
        END DO
        IF(IP.GT.0) THEN
          WRITE(LTT,611) '                                        '
          WRITE(LOU,611) '                                        '
        END IF
      END DO
      WRITE(LTT,697) WTJT,WTRT,DNJT
      WRITE(LOU,697) WTJT,WTRT,DNJT
C* Try another sample
      GO TO 18
C* All processing completed
   90 STOP 'MATSSF Completed'
C* Error traps
   92 WRITE(LTT,611) ' MATSSF ERROR - No MATSSF.DAT library   '
      STOP 'MATSSF ERROR - Reading library file'
C*
  191 FORMAT(I3,1X,A2,1X,I3,4F10.0,I10)
  192 FORMAT(10X,6F10.0)
  194 FORMAT(F7.4)
  611 FORMAT(2A40)
  612 FORMAT(A80)
  614 FORMAT(BN,F10.0)
  616 FORMAT(A40,F10.4)
  621 FORMAT('0ERROR - Element ',A2,'-',I3,'  not in the library')
  691 FORMAT(///
     1 ' MATSSF - Element & isotope composition '/
     2 ' ====================================== '//
     3 ' Component chemical composition          weight %'/
     4 ' ------------------------------------------------')
  692 FORMAT(1X,A40,F8.4)
  693 FORMAT(1X,'------------------------------------------------'/
     1 34X,'Total  ',F8.4/)
  694 FORMAT(
     1 '   Isotope         weight [%]      number density'
     &,'     SigB    SSF G-fast'/
     2 '                input       norm. [x 1.E24 i/cm3]'
     &,'   [barn]        '/
     3 ' ------------------------------------------------'
     &,'------------------------')
  695 FORMAT(A18,F10.4,A8)
  696 FORMAT(I3,'-',A2,'-',I3,2F11.5,1P,E16.4,E10.3,4A7)
  697 FORMAT(' ------------------------------------------------'
     &      ,'------------------------'/
     &       ' Total    ',2F11.5,1P,E16.4/)
      END
      SUBROUTINE CHKABN(MXE,MXI,IS,NI,AB)
C-Title  : Subroutine CHKABN
C-Purpose: Check that isotopic abundances sum to 100%
      CHARACTER*2 IS(MXE)
      DIMENSION   NI(MXE),AB(MXI,MXE)
      EPS=1.E-4
      DO I=1,MXE
        SAB=0
        NN=NI(I)
        IF(NN.LE.1) CYCLE
C...
C...    PRINT *,IS(I),NN,(AB(J,I),J=1,NN)
C...    IF(I.GT.13) STOP
C...
        DO J=2,NN
          SAB=SAB+AB(J,I)
        END DO
        IF(SAB.LE.0) CYCLE
C*      -- Normalise the isotopic composition TO 100%
        SAB=100/SAB
        IF(ABS(SAB-1).GT.EPS) THEN
          IF(ABS(SAB/100-1).GT.EPS) THEN
            PRINT *,'WARNING - Incomplete abundances for ',IS(I),SAB
            PRINT *,'N,AB',NN,(AB(J,I),J=1,NN)
          END IF
        END IF
        DO J=2,NN
          AB(J,I)=AB(J,I)*SAB
        END DO
      END DO
      RETURN
      END
      FUNCTION GH2R(H2R)
C-Title  : Function GH2R
C-Purpose: Function for evaluation of cord length of a flat-lying wire
      DIMENSION GC(8)
      DATA GC1,GC2,GC3,GC4,GC5,GC6,GC7,GC8/
     &         0.999105,  0.029183, 0.427788, -0.456087,
     &         0.339182, -0.185970, 0.059748, -0.008226 /
      GH2R=H2R*(GC1+H2R*(GC2+H2R*(GC3+H2R*(GC4+H2R*(GC5+H2R*
     &         (GC6+H2R*(GC7+H2R*GC8)))))))
      RETURN
      END
      SUBROUTINE FLXSLD(NG,AMS,SGP,ZG0,EN,FL,XSA,XSE,FLX)
C-Title  : Subroutine FLXSLD
C-Purpose: Solve the group-wise slowing-down equation
C-Description:
C-D  NG   Number of energy points
C-D  AMS  Nuclide (average) mass
C-D  SGP  Potential scattering cross section*Goldstein-Cohen lambda
C-D  ZG0  Background cross section
C-D  FL   Flux at infinite dilution
C-D  XSA  Absorption cross section
C-D  XSE  Scattering cross section*Goldstein-Cohen lambda
C-D  FLX  Perturbed flux (solution)
C-
      DIMENSION EN(NG),FL(NG),XSA(NG),XSE(NG),FLX(NG)
      AL =(AMS-1)/(AMS+1)
      AL =AL*AL
      NG1=NG-1
c...
c...  NR =1
      NR =0
      IF(NR.EQ.1) GO TO 30
c...
C_GZ Slowing-down approximation
      DO IG=2,NG
        JG=NG+1-IG
        J1=JG+1
        SG=0
        Z=0              !auxilliary variable 'z' 
        EG=EN(JG)        !lower energy group limit, inner loop (Eg)
        EG1=EN(J1)       !upper energy group limit, inner loop (Eg+1)
        DI=EG1-EG        !energy group width, outer loop (delta Eg)
        ETOP=EG1/AL      !upper limit for scattering integral 
        DO KG=J1,NG1     !inner loop variable (h)
          EH=EN(KG)      !lower energy group limit, inner loop (Eh)
          IF(EH.GE.ETOP) GO TO 20
          EH1=EN(KG+1)   !upper energy group limit, outer loop (Eh+1)
          DJ=EH1-EH      !energy group width, inner loop (delta Eh)
          IF(EH.LT.(EG/AL))THEN
            IF(EH1.LT.(EG/AL))THEN
              Z=DI/EH1
            ELSE IF(EH1.LT.(EG1/AL))THEN
              Z=AL*ALOG(AL*EH1/EG)+EG1/EH1-AL
            ELSE
              Z=AL*ALOG(EG1/EG)
            END IF
          ELSE
            IF(EH1.LT.(EG1/AL))THEN
              Z=EG1/EH1-EG/EH+AL*ALOG(EH1/EH)
            ELSE
              Z=AL-EG/EH+AL*ALOG(EG1/(AL*EH))
            END IF
          END IF
          SG=SG+FLX(KG)*(XSE(KG)/(1-AL))*((DI/EH-Z)/ALOG(EH1/EH)) !contribution from down-scattering
        END DO
   20   IF(EG1.LT.(EG/AL))THEN
          Z=DI/EG1
        ELSE
          Z=AL*ALOG(AL*EG1/EG)+1-AL
        END IF
        FLX(JG)=(FL(JG)*ZG0 + SG)/
     &  (ZG0+XSA(JG)+XSE(JG)-
     &  (XSE(JG)/(1-AL))*(1-Z/ALOG(EG1/EG)))   !diagonal term
      END DO
      RETURN
C_AT Intermediate resonance approximation
   30 DO IG=2,NG
        JG=NG+1-IG
        J1=JG+1
        SG=0
        DI=EN(J1)-EN(JG)
        ETOP=EN(J1)/AL
        DO KG=J1,NG1
          E1=EN(KG)
          IF(E1.GE.ETOP) GO TO 40
          E2=MIN(EN(KG+1),ETOP)
          DJ=E2-E1
          SG=SG+FLX(KG)*(XSE(KG)/(1-AL))*(DI/DJ)*ALOG(E2/E1)
        END DO
   40   CONTINUE
C...    -- Detailed multigroup flux calculation (ERROR???)
C...    FLX(JG)= (FL(JG)*ZG0 + SG ) / (ZG0+XSA(JG)+XSE(JG))
c...    -- Intermediate resonance approximation
        FLX(JG)=  FL(JG)*(ZG0+SGP)  / (ZG0+XSA(JG)+XSE(JG))
C...    -- WR approximation
c...    FLX(JG)=  FL(JG)*ZG0        / (ZG0+XSA(JG)        )
c...
      END DO
      RETURN
      END
      SUBROUTINE CHLEN(CH,LN,MXLN)
C-Title  : Subroutine CHLEN
C-Purpose: Find string length to the last non-blank character
      CHARACTER*(*) CH
      LN=MXLN
      DO WHILE(CH(LN:LN).EQ.' ' .AND. LN.GT.1)
        LN=LN-1
      END DO
      RETURN
      END
      FUNCTION NUMATM(NE,IS,JS)
C-Title  : NUMATM Function
C-Purpose: Provide index that matches symbol JS in array IS
C-Description:
C-D  NE elements of 2-character array IS are searched to match JS.
C-D  Characters in IS must be alphanumeric and are forced uppercase.
C-D  The test word JS is assumed uppercase by definition. The index
C-D  of the matching array element is the evaluated function NUMATM.
C-D  The function value is zero if no matching string is found.
C-
      CHARACTER*1 C1,C2
      CHARACTER*2 IS(1),JS,CH
      DO 10 J=1,NE
      CH=IS(J)
      C1=CH(1:1)
      C2=CH(2:2)
      IF(C1.GT.'Z') C1=CHAR(ICHAR(C1)-32)
      IF(C2.GT.'Z') C2=CHAR(ICHAR(C2)-32)
      CH(1:1)=C1
      CH(2:2)=C2
      IF(CH.NE.JS) GO TO 10
      NUMATM= J
      RETURN
   10 CONTINUE
      NUMATM=0
      RETURN
      END
      FUNCTION ELMWGT(JA,NI,IA,AM)
C-Title  : ELMWGT Function
C-Purpose: Identify appropriate element/isotope atomic weight
C-Description:
C-D  For a particular element a table AM of atomic weights for 
C-D  isotopes with atomic numbers IA is given. The table contains
C-D  NI entries. For the isotope with mass number JA (JA=0 if
C-D  average atomic weight for the element is requested), find
C-D  the appropriate atomic weight in the table.
C-
      DIMENSION  IA(1),AM(1)
      DO 10 I=1,NI
      IF(IA(I).NE.JA) GO TO 10
      ELMWGT=AM(I)
      RETURN
   10 CONTINUE
      ELMWGT=0.
      RETURN
      END
      SUBROUTINE ISOWGT(JA,NI,IA,AM,AB,WT,DN,WE)
C-Title  : ISOWGT Subroutine
C-Purpose: Distribute element weight fractions over isotopes
C-Description:
C-D  Given the weight-% WE of an element in a component, for an 
C-D  element with NI isotopes having relative isotopic abundances 
C-D  AB [%] and atomic weights AM [amu], distribute and add the 
C-D  corresponding isotope weight fractions into WT. If a specific
C-D  isotope with mass number JA appears on input (for natural
C-D  elements JA=0), the
C-
      DIMENSION  IA(1),AM(1),AB(1),WT(1),DN(1)
C* Avogadro's number (x 1.E-24), Ref.: http://physics.nist.gov/PhysRefData/
      DATA AVN/0.60221415/
      IF(JA.GT.0) GO TO 40
      WEL=ELMWGT(JA,NI,IA,AM)
      DO 20 I=1,NI
      DN(I) = DN(I) + WE * 0.0001*AB(I)*AVN  /WEL
      WT(I) = WT(I) + WE * 0.01  *AB(I)*AM(I)/WEL
   20 CONTINUE
      RETURN
   40 DO 42 I=1,NI
      IF(JA.NE.IA(I)) GO TO 42
      DN(I) = DN(I) + WE * 0.01  *      AVN  /AM(I)
      WT(I) = WT(I) + WE
      RETURN
   42 CONTINUE
      END
      SUBROUTINE MATINP(REC,IR,MR,JS,JA,AF,IER)
C-Title  : MATINP Subroutine
C-Purpose: Identify an element or isotope on an input record
C-Description:
C-D  A character string REC is searched from record IR to MR. It is
C-D  assumed to contain 1 or 2-character long chemical element symbol
C-D  (and optionally - the mass number; delimiter "-" may be used)
C-D  and the element multiplicity, separated by blanks. 
C-D    On exit, the element symbol (2-character string, left justified)
C-D  is given in JS, the mass number in JA and the element/isotope mole
C-D  fraction in AF. If mole fraction is not specified, it is assumed 1.
C-D  For natural elements JA is normally zero. A single element 
C-D  or isotope is extracted in one call. On exit the IR pointer index
C-D  is positioned on the next non-blank, non-numeric character in REC.
C-D
C-D  Examples: "  B 2 O 3"  Boron oxide,
C-D            "Al 2 O 3 "  Aluminium oxide,
C-D            "Si O 2 "
C-D            "B-10 .199 B11 .801" natural boron composition
C-D
C-D  IER error codes: -1 - no data on line
C-D                    0 - normal termination
C-D                    1 - format error, element/isotope undefine
      CHARACTER*2  JS
      CHARACTER*1  CH,REC(1)
      IER=-1
      IC = 1
      JA = 0
      AF = 1.
      JS = '  '
      IF(IR.GT.MR) RETURN
C* Skip leading Blanks
   20 CH=REC(IR)
      IF(CH.NE.' ') GO TO 22
      IF(IR.EQ.MR) RETURN
      IR = IR+1
      GO TO 20
   22 IER= 0
C* Extract symbol JS, Else: on numeric or "-" JA, on blank AF
   30 IF(CH.GE.'a'.AND.CH.LE.'z') CH = CHAR(ICHAR(CH)-32)
      IF(CH.GE.'A'.AND.CH.LE.'Z') GO TO 32
      IF(CH.GE.'0'.AND.CH.LE.'9') GO TO 40
      IF(CH.EQ.' ') GO TO 50
      IF(CH.NE.'-') GO TO 90
      IR = IR+1
      IF(IR.GT.MR) RETURN
      CH = REC(IR)
      IF(CH.EQ.' ') GO TO 50
      GO TO 40
C* Next character of the symbol encountered
   32 IF(IC.GT.2) GO TO 90
      JS(IC:IC)=CH
      IC = IC+1
      IR=IR+1
      IF(IR.GT.MR) RETURN
      CH=REC(IR)
      GO TO 30
C* Extract the mass number JA
   40 JA = 10*JA + (ICHAR(CH)-48)
      IR=IR+1
      IF(IR.GT.MR) RETURN
      CH=REC(IR)
      IF(CH.NE.' ') GO TO 40
C* Skip blanks, then extract the molar fraction
   50 IR=IR+1
      IF(IR.GT.MR) RETURN
      CH=REC(IR)
      IF(CH.EQ.' ') GO TO 50
      IF(CH.GE.'A'.AND.CH.LE.'Z') RETURN
      D =10.
      P = 1.
      AF=0.
   52 IF(CH.EQ.' ') RETURN
      IF(CH.NE.'.') GO TO 54
C*     Process decimal point
      IF(P.LT.1.) GO TO 90
      D =1.
      P =0.1
      IR=IR+1
      IF(IR.GT.MR) RETURN
      CH=REC(IR)
      GO TO 52
C*     Process numeric data
   54 AF = D*AF + P*(ICHAR(CH)-48)
      IF(P.LT.1.) P=0.1*P
      IR=IR+1
      IF(IR.GT.MR) RETURN
      CH=REC(IR)
      GO TO 52
   90 IER=1
      RETURN
      END
      SUBROUTINE RESPOL(S1,S2,S,RI,R1,R2,R,LMAKS)
      LOGICAL LMAKS
C   SCCS id Keywords. Do Not Edit.
      CHARACTER*30 SIDNO
      SIDNO='%W% %G%'
      IF(R1.EQ.R2)GO TO 5
      LMAKS=.FALSE.
      ITER=0
      G1=RI/R1
      G2=RI/R2
      RAW=S1/S2
      T=ALOG(G1)/ALOG(G2)
      D=ALOG(G2/G1)
      WL=ALOG(RAW*T)/D
      WH=ALOG(RAW)/D
 2    W=(WL+WH)/2.0
      G=G2**W-1.0-RAW*(G1**W-1.0)
      IF(ABS(G).LT.0.001)GO TO 4
      ITER=ITER+1
      IF(ITER.GT.50)GO TO 6
      IF(G.LT.0.0)GO TO 3
      WL=W
      GO TO 2
 3    WH=W
      GO TO 2
 4    ET=(G1**W-1.0)*S1
      P=1.0/W
      R=RI*(S/(S+ET))**P
      RETURN
 5    R=R1
      RETURN
 6    LMAKS=.TRUE.
      RETURN
      END
      SUBROUTINE RDTEXT(LEF,MAT,MF,MT,REC,IER)
C-Title  : RDTEXT Subroutine
C-Purpose: Read a text record to an ENDF file
      CHARACTER*66  REC
      READ (LEF,40,END=81,ERR=82) REC,MAT,MF,MT
      IER=0
      RETURN
   81 IER=1
      RETURN
   82 IER=2
      RETURN
   40 FORMAT(A66,I4,I2,I3,I5)
      END
      SUBROUTINE RDHEAD(LEF,MAT,MF,MT,C1,C2,L1,L2,N1,N2,IER)
C-Title  : Subroutine RDHEAD
C-Purpose: Read an ENDF HEAD record
C-Description:
C-D  The HEAD record of an ENDF file is read. The following error
C-D  conditions are trapped by setting the IER flag:
C-D    IER = 0  Normal termination
C-D          1  End-of-file
C-D          2  Read error
C-
      READ (LEF,92) C1,C2,L1,L2,N1,N2,MAT,MF,MT
      RETURN
   92 FORMAT(2F11.0,4I11.0,I4,I2,I3,I5)
      END
      SUBROUTINE RDTAB1(LEF,C1,C2,L1,L2,N1,N2,NBT,INR,EN,XS,NMX,IER)
C-Title  : Subroutine RDTAB1
C-Purpose: Read an ENDF TAB1 record
C-Description:
C-D  The TAB1 record of an ENDF-formatted file is read.
C-D  Error condition:
C-D    IER=1  End-of-file
C-D        2  Read error
C-D       -8  WARNING - Numerical underflow (<E-36)
C-D        8  WARNING - Numerical overflow  (>E+36)
C-D        9  WARNING - Available field length exceeded, NMX entries read.
C-
      DOUBLE PRECISION EE(3),XX(3)
      DIMENSION    NBT(1),INR(1)
      DIMENSION    EN(NMX), XS(NMX)
C*
      IER=0
      READ (LEF,902,END=100,ERR=200) C1,C2,L1,L2,N1,N2
      READ (LEF,903,END=100,ERR=200) (NBT(J),INR(J),J=1,N1)
      JP=N2
      IF(N2.GT.NMX) THEN
        JP=NMX
        IER=9
      END IF
      JR=(JP+2)/3
      J=0
      DO K=1,JR
        READ(LEF,904,END=100,ERR=200) (EE(M),XX(M),M=1,3)
        DO M=1,3
          J=J+1
          IF(J.LE.JP) THEN
            IF(ABS(XX(M)).LT.1E-36) THEN
              XX(M)=0
C...          IER=-8
            ELSE IF(ABS(XX(M)).GT.1.E36) THEN
              XX(M)=1E36
              IER=8
            END IF
            EN(J)=EE(M)
            XS(J)=XX(M)
          END IF
        END DO
      END DO
      RETURN
  100 IER=1
      RETURN
  200 IER=2
      RETURN
C*
  902 FORMAT(2F11.0,4I11)
  903 FORMAT(6I11)
  904 FORMAT(6F11.0)
      END
      FUNCTION GTSLAB(XI)
C-Title  : Function GTSLAB
C-Purpose: Self-shielding factor for a slab according to De Corte
      EPS=1E-6
      G=0
      X=1
      F=1
c...  K=NINT(5*XI)+10
      K=1000
      DO N=1,K
        F=F*N
        X=-X*XI
        D=X/(N*F)
        G=G+D
        IF(ABS(D/G) .LT. EPS) EXIT
      END DO

      PRINT *,'N',N

      G=-(0.577215 +ALOG(XI) + G)
      GTSLAB=( 1 - (1-XI)*EXP(-XI) - XI*XI*G )/(2*XI)
      RETURN
      END
      FUNCTION GTSPHERE(XI)
C-Title  : Function GTSPHERE
C-Purpose: Self-shielding factor for a sphere according to De Corte
      IF(XI.LE.0.003) THEN
        GTSPHERE=( 1 - 9*XI/8 )
      ELSE
        Y=3*XI/2
        GTSPHERE=( Y*Y - 0.5 + (0.5+Y)*EXP(-2*Y) )*3/(4*Y*Y*Y)
      END IF
      RETURN
      END

