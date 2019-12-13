'****************************************************************************
'
'   Scanner Control SDK Test Program
'
'   Copyright PFU LIMITED 2005-2016
'
'****************************************************************************
Module ModuleScan
    'Win32API define
    Public Declare Function GetPrivateProfileInt Lib "kernel32" Alias "GetPrivateProfileIntA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal nDefault As Integer, ByVal lpFileName As String) As Integer
    Public Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    Public Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer

    'Windows define
    Public Const MAX_PATH As Integer = 260

    'ScanTo
    Public Const TYPE_FILE As Short = 0                  'File
    Public Const TYPE_DIB_HANDLE As Short = 1            'DIB handle
    Public Const TYPE_RAW_IMAGE_HANDLE As Short = 2      'Memory

    'FileType
    Public Const FILE_BMP As Short = 0                   'Windows Bitmap
    Public Const FILE_TIF As Short = 1                   'TIFF
    Public Const FILE_MULTIF As Short = 2                'Multipage TIFF
    Public Const FILE_JPEG As Short = 3                  'JPEG
    Public Const FILE_PDF As Short = 4                   'PDF
    Public Const FILE_MULPDF As Short = 5                'Multipage PDF
    Public Const FILE_MULTIIMAGE_OUTPUT As Short = 6     'Multi Image Output
    Public Const FILE_AUTO_COLOR_DETECTION As Short = 7  'Auto Color Detection

    'CompressionType
    Public Const NO_COMP As Short = 0                    'No compression
    Public Const COMP_MH As Short = 1                    'CCITT G3(1D)
    Public Const COMP_MR2 As Short = 2                   'CCITT G3(2D) Kfactor = 2
    Public Const COMP_MR4 As Short = 3                   'CCITT G3(2D) Kfactor = 4
    Public Const COMP_MMR As Short = 4                   'CCITT G4
    Public Const COMP_JPEG As Short = 5                  'JPEG
    Public Const COMP_OLDJPEG As Short = 6               'OLD JPEG


    'PixelType
    Public Const PIXEL_BLACK_WHITE As Short = 0          'Monochrome
    Public Const PIXEL_GRAYSCALE As Short = 1            'Grayscale
    Public Const PIXEL_RGB As Short = 2                  'RGB

    'Resolution
    Public Const RS_200 As Short = 0                     '200dpi
    Public Const RS_240 As Short = 1                     '240dpi
    Public Const RS_300 As Short = 2                     '300dpi
    Public Const RS_400 As Short = 3                     '400dpi
    Public Const RS_500 As Short = 4                     '500dpi
    Public Const RS_600 As Short = 5                     '600dpi
    Public Const RS_700 As Short = 6                     '700dpi
    Public Const RS_800 As Short = 7                     '800dpi
    Public Const RS_1200 As Short = 9                    '1200dpi
    Public Const RS_CUSTM As Short = 99                  'Custom

    'Background
    Public Const MODE_OFF As Short = 0                   'Invalid
    Public Const MODE_ON As Short = 1                    'Effective
    Public Const MODE_AUTO As Short = 2                  'Automatic

    'Outline
    Public Const NONE As Short = 0                       'Nothing
    Public Const OUTLINE_EMPHASIS_LOW As Short = 1       'Emphasis low
    Public Const OUTLINE_EMPHASIS_MIDIUM As Short = 2    'Emphasis midium
    Public Const OUTLINE_EMPHASIS_HIGH As Short = 3      'Emphasis high
    Public Const OUTLINE_SMOOTH As Short = 4             'Smooth
    Public Const OUTLINE_EDGE_EXTRACT As Short = 5       'Edge extract

    'Halftone
    Public Const DITHER_PATTERN0 As Short = 1            'deep photograph picture
    Public Const DITHER_PATTERN1 As Short = 2            'mixture deep character and photograph
    Public Const DITHER_PATTERN2 As Short = 3            'light photograph picture
    Public Const DITHER_PATTERN3 As Short = 4            'mixture light character and phtograph
    Public Const DITHER_PATTERN_FILE As Short = 5        'halftone pattern file
    Public Const DITHER_ERROR_DIFFUSION As Short = 6     'error diffusion method

    'Gamma
    Public Const GAMMA_SOFT As Short = 1                 'Soft
    Public Const GAMMA_HARD As Short = 2                 'Sharp
    Public Const GAMMA_PATTERN_FILE As Short = 3         'Gamma pattern file
    Public Const GAMMA_CUSTOM As Short = 4               'custom

    'PaperSupply
    Public Const FLATBED As Short = 0                    'Flatbed
    Public Const ADF As Short = 1                        'ADF
    Public Const ADF_DUPLEX As Short = 2                 'ADF(duplex)
    Public Const ADF_BACKSIDE As Short = 3               'ADF(back side)
    Public Const ADF_A3CS As Short = 4                   'A3 CarrierSheet(ADFduplex)
    Public Const ADF_DLCS As Short = 5                   'Double Letter CarrierSheet(ADFduplex)
    Public Const ADF_B4CS As Short = 6                   'B4 CarrierSheet(ADFduplex)
    Public Const ADF_CS As Short = 7                     'CarrierSheet front and back image separately(ADFduplex)

    'PaperSize
    Public Const PSIZE_A3 As Short = 0                   'A3
    Public Const PSIZE_A4 As Short = 1                   'A4
    Public Const PSIZE_A5 As Short = 2                   'A5
    Public Const PSIZE_A6 As Short = 3                   'A6
    Public Const PSIZE_B4 As Short = 4                   'B4(JIS)
    Public Const PSIZE_B5 As Short = 5                   'B5(JIS)
    Public Const PSIZE_B6 As Short = 6                   'B6(JIS)
    Public Const PSIZE_LETTER As Short = 7               'letter
    Public Const PSIZE_LEGAL As Short = 8                'legal
    Public Const PSIZE_EXECUTIVE As Short = 9            'exective
    Public Const PSIZE_DOUBLELETTER As Short = 10        'double letter
    Public Const PSIZE_POSTCARD As Short = 11            'post card
    Public Const PSIZE_PHOTO As Short = 12               'photograph
    Public Const PSIZE_CARD As Short = 13                'card
    Public Const PSIZE_C4 As Short = 15                  'C4
    Public Const PSIZE_C5 As Short = 16                  'C5
    Public Const PSIZE_C6 As Short = 17                  'C6
    Public Const PSIZE_DATA_CUSTOM As Short = 99         'custom
    Public Const PSIZE_INDEX_CUSTOM As Short = 14        'index of list
    Public Const PSIZE_ISO_B4 As Short = 18              'B4(ISO)
    Public Const PSIZE_ISO_B5 As Short = 19              'B5(ISO)
    Public Const PSIZE_ISO_B6 As Short = 20              'B6(ISO)
    Public Const PSIZE_85x170 As Short = 21              '8.5 x 17inch
    Public Const PSIZE_85x340 As Short = 22              '8.5 x 34inch
    Public Const PSIZE_85x1063 As Short = 23             '8.5 x 106.3inch
    Public Const PSIZE_85x1250 As Short = 24             '8.5 x 125inch
    Public Const PSIZE_85x1600 As Short = 25             '8.5 x 160inch
    Public Const PSIZE_85x2150 As Short = 26             '8.5 x 215inch
    Public Const PSIZE_85x2200 As Short = 27             '8.5 x 220inch
    Public Const PSIZE_117x170 As Short = 28             '11.7 x 17inch
    Public Const PSIZE_117x340 As Short = 29             '11.7 x 34inch
    Public Const PSIZE_120x170 As Short = 30             '12 x 17inch
    Public Const PSIZE_120x340 As Short = 31             '12 x 34inch
    Public Const PSIZE_120x1250 As Short = 32            '12 x 125inch
    Public Const PSIZE_MAX As Short = 33                 'MaxSize
    Public Const PSIZE_120x1063 As Short = 34            '12 x 106.3inch
    Public Const PSIZE_120x1600 As Short = 35            '12 x 160inch
    Public Const PSIZE_120x2150 As Short = 36            '12 x 215inch
    Public Const PSIZE_120x2200 As Short = 37            '12 x 220inch

    'JobControl
    Public Const INCLUDE_AND_CONTINUE As Short = 1       'A special paper/Patch Code paper is read and it continues
    Public Const INCLUDE_AND_STOP As Short = 2           'A special paper/Patch Code paper is read and stopped
    Public Const EXCLUDE_AND_CONTINUE As Short = 3       'A special paper/Patch Code paper is skipped and it continues
    Public Const EXCLUDE_AND_STOP As Short = 4           'A special paper/Patch Code paper is skipped and stopped

    'OvweWrite
    Public Const OW_OFF As Short = 0                     'It does not overwrite
    Public Const OW_ON As Short = 1                      'It overwrites
    Public Const OW_CONFIRM As Short = 2                 'A check message is displayed

    'Orientation
    Public Const PORTRAIT As Short = 0                   'The direction of length
    Public Const LANDSCAPE As Short = 1                  'Transverse direction

    'Rotation
    Public Const RT_NONE As Short = 0                    'It does not rotate
    Public Const RT_R90 As Short = 1                     '90 right rotation
    Public Const RT_R180 As Short = 2                    '180-degree rotation
    Public Const RT_R270 As Short = 3                    '270 right rotation
    Public Const RT_AUTO As Short = 4                    'auto

    'AutoSeparation
    Public Const AS_OFF As Short = 0                     'Automatic picture domain separation processing is not performed
    Public Const AS_ON As Short = 1                      'Automatic picture domain separation processing is performed

    'SEE
    Public Const SEE_OFF As Short = 0                    'Selection emphasis processing is not performed
    Public Const SEE_ON As Short = 1                     'Selection emphasis processing is performed

    'Report
    Public Const RP_OFF As Short = 0                     'A result is not reported
    Public Const RP_DISPLAY As Short = 1                 'A result displays on a screen
    Public Const RP_FILE As Short = 2                    'A result outputs to a file
    Public Const RP_DISPLAY_FILE As Short = 3            'A result displays on a screen and outputs to a file

    'JpegQuarity
    Public Const COMP_JPEG1 As Short = 0                 'Jpeg compression ratio 1
    Public Const COMP_JPEG2 As Short = 1                 'Jpeg compression ratio 2
    Public Const COMP_JPEG3 As Short = 2                 'Jpeg compression ratio 3
    Public Const COMP_JPEG4 As Short = 3                 'Jpeg compression ratio 4
    Public Const COMP_JPEG5 As Short = 4                 'Jpeg compression ratio 5
    Public Const COMP_JPEG6 As Short = 5                 'Jpeg compression ratio 6
    Public Const COMP_JPEG7 As Short = 6                 'Jpeg compression ratio 7

    'ScanMode
    Public Const SM_NORMAL As Short = 0                  'Normal Scan

    'Filter
    Public Const FILTER_GREEN As Short = 0               'Green
    Public Const FILTER_RED As Short = 1                 'Red
    Public Const FILTER_BLUE As Short = 2                'Blue
    Public Const FILTER_OFF As Short = 3                 'No color
    Public Const FILTER_WHITE As Short = 4               'White

    'ThreshholdCurve
    Public Const TH_CURVE1 As Short = 0                  'light
    Public Const TH_CURVE2 As Short = 1                  'little light
    Public Const TH_CURVE3 As Short = 2                  'usually1
    Public Const TH_CURVE4 As Short = 3                  'usually2
    Public Const TH_CURVE5 As Short = 4                  'little deep
    Public Const TH_CURVE6 As Short = 5                  'deep
    Public Const TH_CURVE7 As Short = 6                  'usually
    Public Const TH_CURVE8 As Short = 7                  'deepest

    'GammaCurve
    Public Const GM_CHARREC1 As Short = 0                'For character recognition1
    Public Const GM_CHARREC2 As Short = 1                'For character recognition2
    Public Const GM_IMAGE1 As Short = 2                  'For deep picture
    Public Const GM_IMAGE2 As Short = 3                  'equal division

    'OverScan
    Public Const OVERSCAN_OFF As Short = 0               'OverScan is not performed
    Public Const OVERSCAN_ON As Short = 1                'OverScan is performed

    'Unit
    Public Const UNIT_INCHES As Short = 0                'Inches
    Public Const UNIT_CENTIMETERS As Short = 1           'Centimeters
    Public Const UNIT_PIXELS As Short = 2                'Pixels

    'Sharpness
    Public Const SH_NONE As Short = 0                    'None

    'PunchHoleRemoval
    Public Const PHR_DO_NOT_REMOVE As Short = 0          'Do not remove

    'PunchHoleRemovalMode
    Public Const PHRM_STANDARD As Short = 0              'Standard

    'EndorserFont
    Public Const EDF_HORIZONTAL As Short = 0             'Horizontal

    'EndorserDialog
    Public Const EDD_OFF As Short = 0                    'OFF

    'JobControlMode
    Public Const JCM_SPECIAL_DOCUMENT As Short = 0       'Special Document

    'AIQCResult
    Public Const AR_BADIMAGE As String = "Bad Image"
    Public Const AR_NOERROR As String = "No Error"

    'FrontBackMergingLocation
    Public Const FBML_RIGHT As Short = 3                 'Right

    'FrontBackMergingRotation
    Public Const FBMR_NONE As Short = 0                  'It does not rotate
    Public Const FBMR_R180 As Short = 2                  '180-degree rotation
    Public Const FBMR_INDEX_R180 As Short = 1            'index of list

    'FrontBackMergingTarget
    Public Const FBMT_ALL As Short = 0                   'All

    'FrontBackMergingTargetMode
    Public Const FBMTM_CUSTOM As Short = 1               'Custom
    Public Const FBMTM_CARDSIZE As Short = 2             'CardSize
    Public Const FBMTM_INDEX_CUSTOM As Short = 0         'index of list(Custom)
    Public Const FBMTM_INDEX_CARDSIZE As Short = 1       'index of list(CardSize)

    'FrontBackMergingTargetSize
    Public Const FBMTG_DEFAULT As Single = 1             '1 inch

    'BarcodeDirection
    Public Const BD_HORIZONTAL_VERTICAL As Short = 2     'Horizontal & Vertical

    'BarcodeType
    Public Const BA_EAN8 As Integer = 1                    'EAN 8
    Public Const BA_EAN13 As Integer = 2                   'EAN 13
    Public Const BA_CODE3OF9 As Integer = 4                'Code 3 of 9
    Public Const BA_CODE128 As Integer = 8                 'Code 128
    Public Const BA_ITF As Integer = 16                    'ITF
    Public Const BA_UPCA As Integer = 32                   'UPC-A
    Public Const BA_CODABAR As Integer = 64                'Codabar
    Public Const BA_PDF417 As Integer = 128                'PDF417
    Public Const BA_QRCODE As Integer = 256                'QR Code
    Public Const BA_DATAMATRIX As Integer = 512            'Data Matrix
    Public Const BA_AZTECCODE As Integer = 1024            'Aztec Code

    Public Const BA_STR_EAN8 As String = "EAN 8"           ' EAN 8
    Public Const BA_STR_EAN13 As String = "EAN 13"         ' EAN 13
    Public Const BA_STR_CODE3OF9 As String = "Code 3 of 9" ' Code 3 of 9
    Public Const BA_STR_CODE128 As String = "Code 128"     ' Code 128
    Public Const BA_STR_ITF As String = "ITF"              ' ITF
    Public Const BA_STR_UPCA As String = "UPC-A"           ' UPC-A
    Public Const BA_STR_CODABAR As String = "Codabar"      ' Codabar
    Public Const BA_STR_PDF417 As String = "PDF417"        ' PDF417
    Public Const BA_STR_QRCODE As String = "QR Code"       ' QR Code
    Public Const BA_STR_DATAMATRIX As String = "Data Matrix" ' Data Matrix
    Public Const BA_STR_AZTECCODE As String = "Aztec Code" ' Aztec Code

    'Barcode Rotation
    Public Const BA_RT_0 As Integer = 0                    ' 0
    Public Const BA_RT_90 As Integer = 1                   ' 90
    Public Const BA_RT_180 As Integer = 2                  ' 180
    Public Const BA_RT_270 As Integer = 3                  ' 270
    Public Const BA_RT_X As Integer = 4                    ' Uncertaity

    Public Const BA_RT_STR_0 As String = "0"
    Public Const BA_RT_STR_90 As String = "90"
    Public Const BA_RT_STR_180 As String = "180"
    Public Const BA_RT_STR_270 As String = "270"
    Public Const BA_RT_STR_X As String = "Uncertainty"

    'PatchCodeDirection
    Public Const PD_VERTICAL As Short = 1                  ' Vertical

    'PathCodeDirection
    Public Const PA_PATCH1 As Integer = 1                  ' Patch 1
    Public Const PA_PATCH2 As Integer = 2                  ' Patch 2
    Public Const PA_PATCH3 As Integer = 4                  ' Patch 3
    Public Const PA_PATCH4 As Integer = 8                  ' Patch 4
    Public Const PA_PATCH6 As Integer = 32                 ' Patch 6
    Public Const PA_PATCHT As Integer = 256                ' Patch T

    Public Const PA_STR_PATCH1 As String = "Patch 1"       ' Patch 1
    Public Const PA_STR_PATCH2 As String = "Patch 2"       ' Patch 2
    Public Const PA_STR_PATCH3 As String = "Patch 3"       ' Patch 3
    Public Const PA_STR_PATCH4 As String = "Patch 4"       ' Patch 4
    Public Const PA_STR_PATCH6 As String = "Patch 6"       ' Patch 6
    Public Const PA_STR_PATCHT As String = "Patch T"       ' Patch T

    'EdgeFiller
    Public Const EF_OFF As Integer = 0                   'Off

    Public Const INCH254 As Double = 2.54



    'error code
    Public Const RC_NOT_DS_FJTWAIN As Integer = 2          'It is not FJTWAIN32 driver
    Public Const RC_FAILURE As Integer = -1                'error
    Public Const RC_SUCCESS As Integer = 0                 'success
    Public Const RC_CANCEL As Integer = 1                  'cancel


    'other
    Public intOrientation As Integer            'Orientation
    Public intReport As Integer                 'Report
    Public strReportFile As String              'ReportFile
    Public strOutputResult As String            'OutputResult

    Public blnIsExistsFB As Boolean             'IsExistFB
    Public strImageScanner As String            'ImageScanner
    Public intPageCount As Integer              'PageCount
    Public intErrorCode As Integer              'ErrorCode
    Public strTwainDS As String                 'TwainDS

    Public blnFjtwn As Boolean                  'Be FJTWAIN32 Driver or not?
    Public blnOpenScanner As Boolean            'It is open scanner or not?
    Public strFilePath As String                'File path
    Public strDirPath As String                 'Directory path

    Public bInitialFileRead As Boolean = False  'InitialFileRead() Flag
    Public PreviousUnit As Short                'The state before change of Unit ComboBox
    Public PreviousReso As Short                'The state before change of Resolution ComboBoX

    Public bCancelScan As Boolean = False       'CancelScan Flag

    Public intProfileNum() As Integer           'ProfileNumber

    Public Const BR_DETECTION As Short = 1
    Public Const BR_NONDETECTION As Short = 0

    Public Const BR_STR_DETECTION As String = "Detection"
    Public Const BR_STR_NONDETECTION As String = "Non-detection"

    Public Const MR_DETECTION As String = "Detection"
    Public Const MR_NONDETECTION As String = "Non-detection"

    Public Const SINGLE_DEFAULT As Single = 0             '0 SINGLE初始值
End Module
