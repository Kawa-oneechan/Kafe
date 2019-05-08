Attribute VB_Name = "mGame"
Option Explicit

#Const VAMPIRESTYLE = 0

Private Declare Function GetAsyncKeyState Lib "user32.dll" (ByVal vKey As Long) As Integer
Public Declare Sub mouse_event Lib "user32.dll" (ByVal dwFlags As Long, ByVal dx As Long, ByVal dy As Long, ByVal cButtons As Long, ByVal dwExtraInfo As Long)
Public Declare Function ShowCursor Lib "user32.dll" (ByVal bShow As Long) As Long
Public Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As Any, ByVal lpDefault As String, ByVal lpReturnedString As String, ByVal nSize As Long, ByVal lpFileName As String) As Long
Public Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As Any, ByVal lpString As Any, ByVal lpFileName As String) As Long
Public Declare Function GetTickCount Lib "kernel32" () As Long
Private Declare Function GetTempFileName Lib "kernel32.dll" Alias "GetTempFileNameA" (ByVal lpszPath As String, ByVal lpPrefixString As String, ByVal wUnique As Long, ByVal lpTempFileName As String) As Long
Private Declare Function GetTempPath Lib "kernel32.dll" Alias "GetTempPathA" (ByVal nBufferLength As Long, ByVal lpBuffer As String) As Long

Private Const MaxDDQE = 2048 '4096

Private Enum eDDQEKinds
  dqekNothing = 0
  dqekBlt = 1
  dqekBltFx = 2
  dqekBltFast = 3
  dqekRect = 4
  dqekCardboard = 5
End Enum

Private Type tDDQueueElement
  Kind As eDDQEKinds
  srcRect As RECT
  destRect As RECT
  DDS As DirectDrawSurface7
  BltFx As DDBLTFX
  x1 As Long
  y1 As Long
  x2 As Long
  y2 As Long
  FillColor As Long
  BorderColor As Long
  flags As Long
  CardboardLucent As Single
  CardboardRotate As Single
  cardboardBlendMode As Integer
End Type

Private DDQueue(MaxDDQE) As tDDQueueElement
Private DDQueueSize As Integer

Private Type BITMAPFILEHEADER
  bfType As Integer
  bfSize As Long
  bfReserved1 As Integer
  bfReserved2 As Integer
  bfOffBits As Long
End Type

Private Type BITMAPINFOHEADER
  biSize As Long
  biWidth As Long
  biHeight As Long
  biPlanes As Integer
  biBitCount As Integer
  biCompression As Long
  biSizeImage As Long
  biXPelsPerMeter As Long
  biYPelsPerMeter As Long
  biClrUsed As Long
  biClrImportant As Long
End Type

Private Type RGBQUAD
  rgbBlue As Byte
  rgbGreen As Byte
  rgbRed As Byte
  rgbReserved As Byte
End Type

Public Const MAXFIGHTERS = 16
Public Const MAXPLAYERS = 2
Public Const MAXANIMS = 16
Public Const MAXFRAMES = 24
Public Const MAXRECTS = 4
Public Const MAXBACKGROUNDS = 16
Public Const MAXPROPS = 8
Public Const MAXTAUNTS = 4
Public Const MAXSPECIALS = 4
Public Const MAXSTORYPARAGRAPHS = 8
Public Const MAXPARTICLES = 512
Public Const GROUND = 294
Public Const SPECIALTIME = 20
Public Const BRAINSPEED = 2
Public Const MAXSCRIPTKEYS = 256

Public Enum eScriptModes
  smNormal = 0
  smSay = 1
  smWait = 2
End Enum

Public dx As New DirectX7
Public dxe As DirectXEvent
Public dD As DirectDraw7
Public d3d As Direct3D7
Public d3dev As Direct3DDevice7
Public di As DirectInput
Public didevK As DirectInputDevice
Public didevJ(MAXPLAYERS) As DirectInputDevice
Public didevenum As DirectInputEnumDevices

Public DDS_primary As DirectDrawSurface7
Public ddsd_primary As DDSURFACEDESC2
Public DDS_back As DirectDrawSurface7
Public ddsd_back As DDSURFACEDESC2
Public ddClipper As DirectDrawClipper

Public Enum eController
  fcPlayer1 = 0
  fcPlayer2 = 1
  fcCPU = 2
End Enum

Public Enum eBGAnimTypes
  bgaStill
  bgaLoop
  bgaPlax
  bgaPlax2
  bgaSimpleScroll
End Enum

Public Enum eHitrectKinds
  hrNotUsed = 0
  hrVulnerable = 1
  hrDamaging = 2
End Enum

Enum eWipes
  wipeRandom = -1
  wipeStretchDown = 0
  wipeShrinkUp = 1
  wipeZoomOut = 2
  wipeTV = 3
  wipeBlack = 4
End Enum

Enum ePropKinds
  pkNotUsed = 0
  pkSimpleAnimation
  pkCrashTarget
  pkReacter
  pkTracker
  pkFountain
End Enum

Enum eSpecialLimits
  slNoLimits = 0
  slOnlyOnGround = 1
  slOnlyInAir = 2
End Enum

Public Type tKeypad
  bLeft As Boolean
  bRight As Boolean
  bUp As Boolean
  bDown As Boolean
  b1 As Boolean
  b2 As Boolean
  b3 As Boolean
  b4 As Boolean
  b5 As Boolean
  b6 As Boolean
  bOnePunch As Boolean
  bTwoPunches As Boolean
  bThreePunches As Boolean
  bOneKick As Boolean
  bTwoKicks As Boolean
  bThreeKicks As Boolean
  bStart As Boolean

  codeLeft As Integer
  codeRight As Integer
  codeUp As Integer
  codeDown As Integer
  codeB1 As Integer
  codeB2 As Integer
  codeB3 As Integer
  codeB4 As Integer
  codeB5 As Integer
  codeB6 As Integer
  codeStart As Integer
End Type

Public Type tHitrect
  Kind As eHitrectKinds
  x1 As Long
  x2 As Long
  y1 As Long
  y2 As Long
End Type

Public Type tFrame
  Left As Long
  Top As Long
  Width As Long
  Height As Long
  OffX As Long
  OffY As Long
  ImpulseX As Single
  ImpulseY As Single
  LoopBack As Integer
  Sound As Integer
  Rects(MAXRECTS) As tHitrect
End Type

Public Type tAnimation
  Frames(MAXFRAMES) As tFrame
  FrameC As Integer
  FallTo(10) As Integer
  CancelTo(10) As Integer
  AnimName As String
  RecoveryTime As Integer
End Type

Public Type tSpecial
  FriendlyName As String
  IsSuper As Boolean
  Anim As Integer
  Limits As eSpecialLimits
  LevelsNeeded As Integer
  KeySequence As String
End Type

Public Type tFighter
  FriendlyName As String
  FullName As String
  HomeArea As String
  HomeLand As String
  Height As Single
  Weight As Single
  FilenameBase As String
  Controller As eController
  Anims(MAXANIMS) As tAnimation
  Anim As Integer
  Frame As Integer
  FacingLeft As Boolean
  SurfaceID As Integer
  Health As Integer
  HomeArena As Integer
  X As Long
  Y As Long
  VelX As Single
  VelY As Single
  Taunts(MAXTAUNTS) As String
  Specials(MAXSPECIALS) As tSpecial
  LastKeys As String
  LastKeysTimer As Long
  Recolor As Integer
  Locked As Boolean
  FSBShortName As Integer
  FSBLongName As Integer
  RecoveryDelay As Integer
End Type

Public Type tStoryParagraph
  Text As String
  Image As String
End Type

Public Type tStory
  NumPars As Integer
  Music As String
  Background As Integer
  Paragraphs(MAXSTORYPARAGRAPHS) As tStoryParagraph
End Type

Public Type tBackground
  DDS As DirectDrawSurface7
  DDSD As DDSURFACEDESC2
  AnimType As eBGAnimTypes
  AnimFrame As Long
  AnimTimer As Long
  AnimSpeed As Long
  FullscreenLoopFrames As Long
  FriendlyName As String
  FloorPlaneStart As Long
  filename As String
  id As Long
End Type

Public Type tRosterOpponent
  FriendlyName As String 'find 'em by FriendlyName match-up to Fighters array
  BackgroundOverrule As Integer 'zero means opponent's home arena
  MusicOverrule As String '"" means use fighter's own song
  MachinemaScript As String
End Type

Public Type tSurface
  DDS As DirectDrawSurface7
  DDSD As DDSURFACEDESC2
  r As RECT
End Type

Public Type tProp
  SurfaceID As Integer
  FilenameBase As String
  Anims(MAXANIMS / 2) As tAnimation
  X As Long
  Y As Long
  Anim As Integer
  Frame As Integer
  InTheBack As Boolean
  FacingLeft As Boolean
  FaceTowards As Integer
  Kind As ePropKinds
  target As Integer
  BoundTo As Long
End Type

Public Type tParticle
  X As Long
  Y As Long
  Frame As Long
  Lifetime As Long
  DeltaH As Single
  DeltaV As Single
  GroundLevel As Single
End Type

Public Type tBrain
'         .------------ Relationship
'        |    .-------- Distance
'        |   |   .----- Possibilities
  Matrix(30, 3, 4) As Integer
  TickTimer As Integer 'natural slowdown
  Anger As Integer
End Type
Public Brains(MAXFIGHTERS) As tBrain

Public Particles(MAXPARTICLES) As tParticle
Public FighterSurfs(MAXFIGHTERS * 2) As tSurface
Public PropSurfs(MAXPROPS) As tSurface
Public HealthBars As tSurface
Public Fonts As tSurface
Public Signs As tSurface
Public BarPortraits(MAXPLAYERS) As tSurface
Public WinIcons As tSurface
Public ParticleSurf As tSurface

Public fighterpool(MAXFIGHTERS) As tFighter
Public fightersinpool As Integer
Public players(MAXPLAYERS) As tFighter
Public backgrounds(MAXBACKGROUNDS) As tBackground
Public props(MAXPROPS) As tProp
Public Keypad(MAXPLAYERS) As tKeypad
Public RosterOpponents(MAXFIGHTERS, 16) As tRosterOpponent
Public Stories(MAXFIGHTERS, 1) As tStory

Public BGProps(MAXPROPS / 4) As tProp
Public FTProps(MAXPROPS / 4) As tProp

Public Background As tBackground
Public SpazzBackground As tBackground
Public SpazzTimer As Long
Public id1 As Integer, id2 As Integer
Public Scores(MAXPLAYERS) As Long
Public FSBHandles(MAXPLAYERS) As Long
Public AnnouncerFSB As Long
Public EffectsFSB As Long

Public X As DOMDocument30
Public MusicOn As Boolean
Public SoundOn As Boolean
Public GamepadsOn As Boolean
Public MetricSystem As Boolean
Public FullScreen As Boolean

Public r1 As RECT
Public keyboard As DIKEYBOARDSTATE
Public oldkeyboard(255) As Byte
Public newkeyboard(255) As Byte
Public numpads As Byte
Public playerpads(MAXPLAYERS) As Integer
Public NumPlayers As Byte
Public lastWinner As Integer
Public WantedRecolor(MAXPLAYERS) As Integer
Public LastSuper As String

'Single player story progress
Public WinsSoFar As Integer
Public GameOver As Boolean
Public NumOpponents As Integer
Public HereComesANewChallenger As Boolean
Public fromMachinema As Boolean

Public framecount As Long
Public TimeLeft As Integer
Public CameraX As Long
Public CameraLocked As Boolean
Public timeperround As Integer
Public speed As Integer
Public scalar As Single

Public systemsoundHandles(32) As Long
Public songHandle As Long
Public streamChannel As Long
Public currentSong As String
Public ShowDebugInfo As Boolean
Public FPS As Long
Public FPSc As Long

Public Type tGeneralStats
  GamesStarted As Long
  GamesWon As Long
  GamesLost As Long
  UnlockedMalkovic As Boolean
  RoundsPlayed As Long
End Type
Public WinsTable(MAXFIGHTERS, MAXFIGHTERS, 3) As Integer
Public GeneralStats As tGeneralStats

Public Type tMusicVolume
  filename As String
  volume As Integer
End Type
Public MusicVolume(64) As tMusicVolume

Public Fun As Integer

Public Const OVERRIDE = "override\"
Public Const BLOBFILE = "blob"

Public Type tFileTableEntry
  filename As String * 32
  offset As Long
  Size As Long
End Type

Public FileTable(256) As tFileTableEntry
Public FileTableSize As Integer
Public TotalBlobSize As Long

Public Function LoadFile(filename As String)
  Dim i As Integer, ff As Integer, b() As Byte
  If Dir(OVERRIDE & filename) <> "" Then
    LoadFile = OVERRIDE & filename
    Debug.Print "Load " & filename & " from Override"
    Exit Function
  End If
  If FileTable(0).offset = 0 Then
    'Debug.Print "Warning: Blob distorted. Reloading..."
    ff = FreeFile
    Open BLOBFILE For Binary As ff
    Get ff, , FileTable()
    Close ff
  End If
  For i = 0 To 255
    If filename = Trim(FileTable(i).filename) Then
      ReDim b(FileTable(i).Size) As Byte
      ff = FreeFile
      Open BLOBFILE For Binary As ff
      Get ff, FileTable(i).offset, b()
      Close ff
      
      Dim path As String
      Dim newfile As String
      path = String(260, " ")
      newfile = String(260, " ")
      GetTempPath 260, path
      path = Trim(path)
      GetTempFileName path, "KFE", 0, newfile
      newfile = Trim(newfile)
      newfile = Left(newfile, Len(newfile) - 1)
      
      Open newfile For Binary As ff
      Put ff, , b()
      Close ff
      LoadFile = newfile
      Debug.Print "Load " & filename & " from Blob"
      Exit Function
    End If
  Next i
  Open "stderr.txt" For Append As #99
  Print #99, "Warning: Can't find " & filename & "!"
  Close #99
  Debug.Print "Warning: Can't find " & filename & "!"
  LoadFile = "404." & Right(filename, 3)
End Function

Public Sub GetFilePlace(ByRef filename As String, ByRef offset As Long, ByRef Size As Long)
  Dim i As Integer, ff As Integer
  If Dir(OVERRIDE & filename) <> "" Then
    filename = OVERRIDE & filename
    offset = 0
    Size = 0
    Exit Sub
  End If
  If FileTable(0).offset = 0 Then
    ff = FreeFile
    Open BLOBFILE For Binary As ff
    Get ff, , FileTable()
    Close ff
  End If
  For i = 0 To 255
    If filename = Trim(FileTable(i).filename) Then
      filename = BLOBFILE
      offset = FileTable(i).offset - 1
      Size = FileTable(i).Size
      Exit Sub
    End If
  Next i
  Open "stderr.txt" For Append As #99
  Print #99, "Warning: Can't find " & filename & "!"
  Close #99
  filename = "404." & Right(filename, 3)
End Sub

Public Sub KillFile(filename As String)
  If Right(filename, 3) = "tmp" Then Kill filename
End Sub

Public Sub KillAllTemps()
  Close
  Dim path As String
  path = String(260, " ")
  GetTempPath 260, path
  path = Trim(path)
  path = Left(path, Len(path) - 1)
  Dim s As String
  Do
    s = Dir(path & "\KFE*.tmp")
    If s <> "" Then
      Kill path & s
    Else
      Exit Do
    End If
  Loop
End Sub

'---------------------------------
'  BUFFERED DIRECTDRAW DRAWING!!
'---------------------------------
Private Sub QFlush()
  DDQueueSize = 0
End Sub
Private Sub QProcess()
  Dim i As Long
  On Error Resume Next
  For i = 0 To DDQueueSize - 1
    With DDQueue(i)
      If .Kind = dqekNothing Then Exit Sub
      
      If .Kind = dqekBlt Then DDS_back.Blt .destRect, .DDS, .srcRect, .flags
      If .Kind = dqekBltFast Then DDS_back.BltFast .x1, .y1, .DDS, .srcRect, .flags
      If .Kind = dqekBltFx Then DDS_back.BltFx .destRect, .DDS, .srcRect, .flags, .BltFx
      
      If .Kind = dqekRect Then
        DDS_back.SetForeColor .BorderColor
        DDS_back.SetFillColor .FillColor
        DDS_back.DrawBox .x1, .y1, .x2, .y2
      End If
      
      If .Kind = dqekCardboard Then
        QRenderCardboardNow DDQueue(i)
      End If
    End With
  Next
  On Error GoTo 0
End Sub

Private Sub QBlt(destRect As RECT, DDS As DirectDrawSurface7, srcRect As RECT, flags As CONST_DDBLTFLAGS)
  With DDQueue(DDQueueSize)
    .Kind = dqekBlt
    .destRect = destRect
    Set .DDS = DDS
    .srcRect = srcRect
    .flags = flags
  End With
  DDQueueSize = DDQueueSize + 1
End Sub

Private Sub QBltFast(dx As Long, dy As Long, DDS As DirectDrawSurface7, srcRect As RECT, trans As CONST_DDBLTFASTFLAGS)
  With DDQueue(DDQueueSize)
    .Kind = dqekBltFast
    .x1 = dx
    .y1 = dy
    Set .DDS = DDS
    .srcRect = srcRect
    .flags = trans
  End With
  DDQueueSize = DDQueueSize + 1
End Sub

Private Sub QBltFx(destRect As RECT, DDS As DirectDrawSurface7, srcRect As RECT, flags As CONST_DDBLTFLAGS, BltFx As DDBLTFX)
  With DDQueue(DDQueueSize)
    .Kind = dqekBltFx
    .destRect = destRect
    Set .DDS = DDS
    .srcRect = srcRect
    .flags = flags
    .BltFx = BltFx
  End With
  DDQueueSize = DDQueueSize + 1
End Sub

Private Sub QDrawBox(x1 As Long, y1 As Long, x2 As Long, y2 As Long, border As Long, fill As Long)
  With DDQueue(DDQueueSize)
    .Kind = dqekRect
    .x1 = x1
    .y1 = y1
    .x2 = x2
    .y2 = y2
    .FillColor = fill
    .BorderColor = border
  End With
  DDQueueSize = DDQueueSize + 1
End Sub

Private Sub QCardboard(destRect As RECT, DDS As DirectDrawSurface7, srcRect As RECT, Optional alpha As Single = 0, Optional angle As Single = 0, Optional bmode As Integer = 0)
  With DDQueue(DDQueueSize)
    .Kind = dqekCardboard
    .destRect = destRect
    Set .DDS = DDS
    .srcRect = srcRect
    .CardboardLucent = alpha
    .CardboardRotate = angle
    .cardboardBlendMode = bmode
  End With
  DDQueueSize = DDQueueSize + 1
End Sub

Private Sub QRenderCardboardNow(q As tDDQueueElement)
  If q.Kind <> dqekCardboard Then
    Debug.Print "Warning: tried to Render a non-cardboard!"
    Exit Sub
  End If
  
  Dim Verts(3) As D3DTLVERTEX
  Dim SurfW As Single
  Dim SurfH As Single
  Dim XCenter As Single
  Dim YCenter As Single
  Dim Radius As Single
  Dim XCor As Single
  Dim YCor As Single
  Dim mycolor As Long
  mycolor = dx.CreateColorRGBA(1, 1, 1, 1)
  
  SurfW = 128
  SurfH = 128
  XCenter = q.destRect.Left + (q.destRect.Right - q.destRect.Left - 1) / 2
  YCenter = q.destRect.Top + (q.destRect.Bottom - q.destRect.Top - 1) / 2
  
  'Vertex zero: Bottom Left
  If q.CardboardRotate = 0 Then
    XCor = q.destRect.Left
    YCor = q.destRect.Bottom
  Else
    XCor = XCenter + (q.destRect.Left - XCenter) * Sin(q.CardboardRotate) + (q.destRect.Bottom - YCenter) * Cos(q.CardboardRotate)
    YCor = YCenter + (q.destRect.Bottom - YCenter) * Sin(q.CardboardRotate) - (q.destRect.Left - XCenter) * Cos(q.CardboardRotate)
  End If
  dx.CreateD3DTLVertex XCor, YCor, 0, 1, mycolor, 0, q.srcRect.Left / SurfW, (q.srcRect.Bottom + 1) / SurfH, Verts(0)
  
  'Vertex one: Top left
  If q.CardboardRotate = 0 Then
    XCor = q.destRect.Left
    YCor = q.destRect.Top
  Else
    XCor = XCenter + (q.destRect.Left - XCenter) * Sin(q.CardboardRotate) + (q.destRect.Top - YCenter) * Cos(q.CardboardRotate)
    YCor = YCenter + (q.destRect.Top - YCenter) * Sin(q.CardboardRotate) - (q.destRect.Left - XCenter) * Cos(q.CardboardRotate)
  End If
  dx.CreateD3DTLVertex XCor, YCor, 0, 1, mycolor, 0, q.srcRect.Left / SurfW, q.srcRect.Top / SurfH, Verts(1)

  'Vertex two: Bottom right
  If q.CardboardRotate = 0 Then
    XCor = q.destRect.Right
    YCor = q.destRect.Bottom
  Else
    XCor = XCenter + (q.destRect.Right - XCenter) * Sin(q.CardboardRotate) + (q.destRect.Bottom - YCenter) * Cos(q.CardboardRotate)
    YCor = YCenter + (q.destRect.Bottom - YCenter) * Sin(q.CardboardRotate) - (q.destRect.Right - XCenter) * Cos(q.CardboardRotate)
  End If
  dx.CreateD3DTLVertex XCor, YCor, 0, 1, mycolor, 0, (q.srcRect.Right + 1) / SurfW, (q.srcRect.Bottom + 1) / SurfH, Verts(2)

  'Vertex three: Top right
  If q.CardboardRotate = 0 Then
    XCor = q.destRect.Right
    YCor = q.destRect.Top
  Else
    XCor = XCenter + (q.destRect.Right - XCenter) * Sin(q.CardboardRotate) + (q.destRect.Top - YCenter) * Cos(q.CardboardRotate)
    YCor = YCenter + (q.destRect.Top - YCenter) * Sin(q.CardboardRotate) - (q.destRect.Right - XCenter) * Cos(q.CardboardRotate)
  End If
  dx.CreateD3DTLVertex XCor, YCor, 0, 1, mycolor, 0, (q.srcRect.Right + 1) / SurfW, q.srcRect.Top / SurfH, Verts(3)

  d3dev.BeginScene

  'Enable alpha-blending
  d3dev.SetRenderState D3DRENDERSTATE_ALPHABLENDENABLE, True
  'Enable color-keying (ColorKey is drawn transparent)
  d3dev.SetRenderState D3DRENDERSTATE_COLORKEYENABLE, True
  d3dev.SetRenderState D3DRENDERSTATE_COLORKEYBLENDENABLE, True
  'Use Alpha Blend One alpha blending if the ABOne variable is true (you can use any variable)
  If q.CardboardLucent = 1 Then
    d3dev.SetRenderState D3DRENDERSTATE_SRCBLEND, D3DBLEND_ONE
    d3dev.SetRenderState D3DRENDERSTATE_DESTBLEND, D3DBLEND_SRCALPHASAT ' D3DBLEND_ONE
    'Or Alpha blend to a certain fade value (0 - 1)
  Else
    d3dev.SetRenderState D3DRENDERSTATE_SRCBLEND, D3DBLEND_SRCALPHA
    d3dev.SetRenderState D3DRENDERSTATE_DESTBLEND, D3DBLEND_INVSRCALPHA
    d3dev.SetRenderState D3DRENDERSTATE_TEXTUREFACTOR, dx.CreateColorRGBA(1, 1, 1, q.CardboardLucent)
    d3dev.SetTextureStageState 0, D3DTSS_ALPHAARG1, D3DTA_TFACTOR
  End If
  
  'Set the texture on the D3D device
  d3dev.SetTexture 0, q.DDS
  d3dev.SetTextureStageState 0, D3DTSS_MIPFILTER, D3DTFP_NONE
  'Draw the triangles that make up our square texture
  d3dev.DrawPrimitive D3DPT_TRIANGLESTRIP, D3DFVF_TLVERTEX, Verts(0), 4, D3DDP_DEFAULT
  'Turn off alphablending after we're done
  d3dev.SetRenderState D3DRENDERSTATE_ALPHABLENDENABLE, False
  
  d3dev.EndScene
End Sub


'Given a player index, this draws a Fighter sprite on the backbuffer, adapted for flipping and offsets.
'ShowBounds will draw a bounding box for the sprite itself and any hitrects.
'TODO: Make true on that bounding box thing. Where'd it go?
Public Sub DrawFighter(fighter As tFighter, Optional ShowBounds As Boolean = False)
  Dim r1 As RECT
  Dim r2 As RECT
  Dim myBltFx As DDBLTFX
 
  With fighter
    With .Anims(.Anim).Frames(.Frame)
      r1.Top = .Top
      r1.Bottom = .Top + .Height
      r1.Left = .Left
      r1.Right = .Left + .Width
      
      r2.Top = fighter.Y - .OffY
      r2.Bottom = r2.Top + .Height
      If fighter.FacingLeft = False Then
        r2.Left = fighter.X - .OffX
      Else
        r2.Left = fighter.X + .OffX - .Width
      End If
      r2.Right = r2.Left + .Width
      
      myBltFx.lDDFX = DDBLTFX_MIRRORLEFTRIGHT
      
      If fighter.FacingLeft Then
        QBltFx r2, FighterSurfs(fighter.SurfaceID + (MAXFIGHTERS * fighter.Recolor)).DDS, r1, DDBLT_KEYSRC + DDBLT_DDFX, myBltFx
      Else
        QBltFast r2.Left, r2.Top, FighterSurfs(fighter.SurfaceID + (MAXFIGHTERS * fighter.Recolor)).DDS, r1, DDBLTFAST_SRCCOLORKEY
      End If
      
      If ShowBounds Then
        Dim i As Integer
        For i = 0 To MAXRECTS
          If .Rects(i).Kind <> hrNotUsed Then
            'DDS_back.SetForeColor IIf(.Rects(i).Kind = hrDamaging, vbRed, vbGreen)
            If fighter.FacingLeft = False Then
              QDrawBox fighter.X - .OffX + .Rects(i).x1, _
                               fighter.Y - .OffY + .Rects(i).y1, _
                               fighter.X - .OffX + .Rects(i).x2, _
                               fighter.Y - .OffY + .Rects(i).y2, &HFF00FF, IIf(.Rects(i).Kind = hrDamaging, vbRed, vbGreen)
            Else
              QDrawBox fighter.X + .OffX - .Rects(i).x1, _
                               fighter.Y - .OffY + .Rects(i).y1, _
                               fighter.X + .OffX - .Rects(i).x2, _
                               fighter.Y - .OffY + .Rects(i).y2, &HFF00FF, IIf(.Rects(i).Kind = hrDamaging, vbRed, vbGreen)
            End If
          End If
        Next i
      End If
    
    End With
  End With
End Sub

'Special Prop edition of DrawFighter. Needs more things to set it apart.
Public Sub DrawProp(prop As tProp, Optional ShowBounds As Boolean = False)
  Dim r1 As RECT
  Dim r2 As RECT
  Dim myBltFx As DDBLTFX
 
  With prop
    If prop.Kind = pkFountain Then
      AddParticle .X, .Y, 4 + (Rnd * 1), -1.55 + (Rnd * (1.55 * 2)), -3 - (Rnd * 2), prop.target
    Else
      With .Anims(.Anim).Frames(.Frame)
        r1.Top = .Top
        r1.Bottom = .Top + .Height
        r1.Left = .Left
        r1.Right = .Left + .Width
        
        r2.Top = prop.Y - .OffY
        r2.Bottom = r2.Top + .Height
        If prop.FacingLeft = False Then
          r2.Left = prop.X - .OffX
        Else
          r2.Left = prop.X + .OffX - .Width
        End If
        r2.Right = r2.Left + .Width
        
        If prop.FacingLeft Then myBltFx.lDDFX = DDBLTFX_MIRRORLEFTRIGHT
          
        QBltFx r2, PropSurfs(prop.SurfaceID).DDS, r1, DDBLT_KEYSRC + DDBLT_DDFX, myBltFx
      End With
    End If
  End With
End Sub

Public Sub ClearProps()

End Sub

'Draws the current background with all required special effects on the right place in the backbuffer.
Public Sub DrawBackground()
  Dim r1 As RECT, r2 As RECT, i As Long, j As Double
  With Background
    If .AnimType = bgaStill Then
      r1.Top = 100
      r1.Bottom = r1.Top + 224
      r1.Left = CameraX
      r1.Right = r1.Left + 384
      r2.Top = 0
      r2.Bottom = 224
      r2.Left = 0
      r2.Right = 384
      'DDS_back.Blt r1, .ddS, r2, DDBLT_DONOTWAIT
      QBlt r1, .DDS, r2, DDBLT_DONOTWAIT
    End If
    If .AnimType = bgaLoop Then
      r1.Top = 100
      r1.Bottom = r1.Top + 224
      r1.Left = CameraX
      r1.Right = r1.Left + 384
      r2.Top = .AnimFrame * 224
      r2.Bottom = r2.Top + 224
      r2.Left = 0
      r2.Right = 384
      .AnimFrame = IIf(.AnimFrame < .FullscreenLoopFrames - 1, .AnimFrame + 1, 0)
      QBlt r1, .DDS, r2, DDBLT_DONOTWAIT
    End If
    If .AnimType = bgaPlax Then
      r1.Top = 100
      r1.Bottom = r1.Top + 224
      r1.Left = CameraX / 1.5
      r1.Right = r1.Left + 1024
      r2.Top = 224 + 64
      r2.Bottom = r2.Top + 224 - 64
      r2.Left = 0
      r2.Right = r2.Left + 768
      QBlt r1, .DDS, r2, DDBLT_DONOTWAIT
      
      r1.Top = 100
      r1.Bottom = r1.Top + 224
      r1.Left = 0
      r1.Right = r1.Left + 1024
      r2.Top = 0
      r2.Bottom = r2.Top + 224
      r2.Left = 0
      r2.Right = r2.Left + 1024
      QBlt r1, .DDS, r2, DDBLT_KEYSRC + DDBLT_DONOTWAIT
    
'      j = 0
'      For i = .FloorPlaneStart To 224
'        r2.Top = i
'        r2.Bottom = r2.Top + 2
'        r2.Left = 0
'        r2.Right = r2.Left + 768
'        r1.Top = 100 + i
'        r1.Bottom = r1.Top + 2
'        r1.Left = CameraX * (j / 10)
'        If r1.Left < 0 Then r1.Left = 0
'        r1.Right = r1.Left + 1024
'        If r1.Right > 1024 Then r1.Right = 1024
'        DDS_back.Blt r1, .dds, r2, DDBLT_DONOTWAIT
'        j = j + 0.2
'      Next i
    
    End If
    If .AnimType = bgaPlax2 Then
      r1.Top = 100
      r1.Bottom = r1.Top + 224
      r1.Left = 0
      r1.Right = r1.Left + 1024
      r2.Top = 224 + 64 + (Sin(CDbl(.AnimFrame) / 4) * 2)
      r2.Bottom = r2.Top + 224 - 74
      r2.Left = 0
      r2.Right = r2.Left + 1024
      QBlt r1, .DDS, r2, DDBLT_DONOTWAIT
      r2.Top = 0
      r2.Bottom = r2.Top + 224
      r2.Left = 0
      r2.Right = r2.Left + 1024 '688
      QBlt r1, .DDS, r2, DDBLT_KEYSRC + DDBLT_DONOTWAIT
      
      .AnimFrame = IIf(.AnimFrame < 64, .AnimFrame + 1, 0)
    End If
    If .AnimType = bgaSimpleScroll Then
      r1.Top = 100
      r1.Bottom = r1.Top + 224
      r1.Left = CameraX
      r1.Right = r1.Left + 384
      r2.Top = 0
      r2.Bottom = 224
      r2.Left = .AnimFrame
      r2.Right = r2.Left + .AnimSpeed
      QBlt r1, .DDS, r2, DDBLT_DONOTWAIT
      .AnimFrame = IIf(.AnimFrame < 384, .AnimFrame + 1, 0)
    End If
  End With
End Sub

Public Sub PrepareRound(Optional roundno As Integer)
  players(0).Health = IIf(roundno = 0, 1, 100)
  players(1).Health = IIf(roundno = 0, 1, 100)
  players(0).Frame = 0
  players(1).Frame = 0
#If VAMPIRESTYLE = 1 Then
  If roundno = 0 Then
#End If
    players(0).X = 300
    players(1).X = 470
    players(0).Y = GROUND
    players(1).Y = GROUND
    players(0).FacingLeft = False
    players(1).FacingLeft = True
    TimeLeft = timeperround '60
#If VAMPIRESTYLE = 1 Then
  End If
#End If
  Dim a As Integer
  For a = 0 To MAXPROPS
    props(a).Anim = 0
    props(a).Frame = 0
  Next a
  LastSuper = ""
End Sub

'The big one. This one handles all the rounds of fighting.
Public Sub GameLoop()
  Dim r2 As RECT, r3 As RECT, r As Integer, timetoact As Boolean, tc As Long
  Dim a As Long, zzz As Long, slomo As Boolean, slomotimer As Integer
  Dim sign As Integer, signtimer As Integer, winner As Integer, wintimer As Integer
  Dim wintypes(MAXPLAYERS, 5) As Integer
  Dim wins(MAXPLAYERS) As Integer, slide As Integer
  sign = 2
  signtimer = 25
  winner = -1
  slide = 34
  
  SpazzTimer = -1
  
  If StopIt = True Then End

  r = 0
  PrepareRound r
  If fromMachinema = False Then
    players(0).Anim = FindAnimByName("stdIntro", players(0))
    players(1).Anim = FindAnimByName("stdIntro", players(1))
  Else
    fromMachinema = False
  End If
  HereComesANewChallenger = False
  
  players(0).Locked = True
  players(1).Locked = True
  
  While Not StopIt
    HandleTimingAndInput
    'framecount = framecount + 1
    If framecount Mod 4 = 1 And sign = 0 Then
      If players(0).Controller = fcCPU Then ComputerAI 0 ', id1
      If players(1).Controller = fcCPU Then ComputerAI 1 ', id2
    End If
    
    If slide Then slide = slide - 1
    
    If NumPlayers = 0 And Keypad(1).bStart Then
      NewChallengerLoop
      HereComesANewChallenger = True
      WipeOut
      Exit Sub
    End If
    
    If framecount Mod 16 = 1 And TimeLeft > 0 And sign = 0 Then TimeLeft = TimeLeft - 1
  
    If Not CameraLocked Then CameraX = ((players(0).X + players(1).X) / 2) - (368 / 2)
    If CameraX < 80 Then CameraX = 80
    If CameraX > 434 Then CameraX = 434
          
    If SpazzTimer > -1 Then
      SpazzTimer = SpazzTimer - 1
      If SpazzTimer = -1 Then Background = SpazzBackground
    End If
          
    DrawBackground
    
    If LastSuper <> "" Then
      If players(0).Anim = 0 Or players(1).Anim = 1 Then LastSuper = ""
    End If
    
    If TimeLeft = 0 And winner = -1 Then
      winner = -2
      PlayFSBSound AnnouncerFSB, ANN_16_TIMEOVER
      sign = 5
      signtimer = 100
      If players(0).Health < players(1).Health Then '2up wins
        'Debug.Print "Time over! " & players(1).FriendlyName & " wins."
        wintypes(1, wins(1)) = 3
        wins(1) = wins(1) + 1
        players(0).Anim = FindAnimByName("stdTimeOut", players(0))
        players(1).Anim = FindAnimByName("stdVictory", players(1))
      ElseIf players(0).Health > players(1).Health Then '1up wins
        'Debug.Print "Time over! " & players(0).FriendlyName & " wins."
        wintypes(0, wins(0)) = 3
        wins(0) = wins(0) + 1
        players(0).Anim = FindAnimByName("stdVictory", players(0))
        players(1).Anim = FindAnimByName("stdTimeOut", players(1))
      Else
        'Debug.Print "Time over! Draw game."
        players(0).Anim = FindAnimByName("stdTimeout", players(0))
        players(1).Anim = FindAnimByName("stdTimeout", players(1))
      End If
      players(0).Locked = True
      players(1).Locked = True
    End If 'timeleft = 0
    
    For a = 0 To MAXPROPS
      If props(a).Kind <> pkNotUsed And props(a).BoundTo = -(Background.id) Then
        HandlePropAnim a, CLng(winner)
        If props(a).InTheBack = True Then
          DrawProp props(a), True
        End If
      End If
    Next a
    
    DrawParticles
    
    For a = 0 To 1
'#If STRETCHEDSHADOWS Then
'      With players(a).Anims(players(a).Anim).Frames(players(a).Frame)
'        r2.Left = 0
'        r2.Top = 8
'        r2.Right = r2.Left + 64
'        r2.Bottom = r2.Top + 8
'        r3.Top = GROUND - 4
'        r3.Bottom = r3.Top + 8
'        r3.Left = IIf(players(a).FacingLeft = False, players(a).X - .OffX, players(a).X + .OffX - .Width)
'        r3.Right = r3.Left + .Width
'        QBlt r3, ParticleSurf.ddS, r2, DDBLT_KEYSRC
'      End With
'#Else
'      r2.Left = 0
'      r2.Top = 8
'      r2.Right = r2.Left + 64
'      r2.Bottom = r2.Top + 8
'      QBltFast players(a).X - 32, GROUND - 4, ParticleSurf.ddS, r2, DDBLTFAST_SRCCOLORKEY
'#End If
      With players(a).Anims(players(a).Anim).Frames(players(a).Frame)
        r2.Left = 0
        r2.Top = 8
        r2.Right = r2.Left + 64
        r2.Bottom = r2.Top + 8
        r3.Top = GROUND - 4
        r3.Bottom = r3.Top + 8
        r3.Left = players(a).X - 32
        r3.Right = r3.Left + 64
        QCardboard r3, ParticleSurf.DDS, r2, 0.5, 0
      End With
    Next a
    
    If players(0).X < players(1).X Then
      DrawFighter players(1) ', True
      DrawFighter players(0) ', True
    Else
      DrawFighter players(0) ', True
      DrawFighter players(1) ', True
    End If
    For a = 0 To MAXPROPS
      If props(a).Kind <> pkNotUsed And props(a).InTheBack = False Then DrawProp props(a), True
    Next a
    
    If slomotimer = 0 Then
      HandleAnimation 0
      HandleAnimation 1
      If slomo = True Then slomotimer = 1
    Else
      slomotimer = slomotimer - 1
    End If
    
    a = DetectHit(0, 1, hrVulnerable, hrVulnerable)
    If a Then
      If players(0).FacingLeft = False Then
        players(0).X = players(0).X - (a * players(1).Weight / (players(0).Weight + players(1).Weight)) 'sd = o * rpw / (lpw + rpw)
        players(1).X = players(1).X + (a * players(0).Weight / (players(0).Weight + players(1).Weight)) 'sd = o * lpw / (lpw + rpw)
      Else
        players(1).X = players(1).X - (a * players(1).Weight / (players(0).Weight + players(1).Weight)) 'sd = o * rpw / (lpw + rpw)
        players(0).X = players(0).X + (a * players(0).Weight / (players(0).Weight + players(1).Weight)) 'sd = o * lpw / (lpw + rpw)
      End If
    End If
    
    If winner = -1 Then
      For a = 0 To 1
        If players(a).Health <= 0 Then
          PlayFSBSound AnnouncerFSB, ANN_09_KNOCKOUT + (Rnd * 1)
          players(a).Health = 0
          players(a).Anim = FindAnimByName("stdKnockedDown", players(a))
          players(IIf(a = 0, 1, 0)).Anim = FindAnimByName("stdVictory", players(IIf(a = 0, 1, 0)))
          winner = IIf(a = 0, 1, 0)
          wintypes(winner, wins(winner)) = IIf(players(winner).Health = 100, 2, 1)
          wins(winner) = wins(winner) + 1
          slomo = True
          slomotimer = 1
          sign = 7
          signtimer = 25
          players(0).Locked = True
          players(1).Locked = True
          
        End If
      Next a
    Else
      players(0).Locked = True
      players(1).Locked = True
      wintimer = wintimer + 1
      If wintimer = 20 And winner > -1 Then
        If NumPlayers = 0 Then 'You win or You Lose depending on who won
          PlayFSBSound AnnouncerFSB, IIf(winner = 0, IIf(players(winner).Health = 100, ANN_11_PERFECT, ANN_12_YOUWIN), ANN_13_YOULOSE)
          sign = IIf(winner = 0, 4, 0)
          signtimer = 100
        Else 'Both players can get You Win
          PlayFSBSound AnnouncerFSB, IIf(players(winner).Health = 100, ANN_11_PERFECT, ANN_12_YOUWIN)
          sign = 4
          signtimer = 100
        End If
      End If
      If wintimer = 75 Then
        wintimer = 0
        slomo = False
        r = r + 1
        lastWinner = -1
        For a = 0 To 1
          If wins(a) = 2 Then 'one of the players has two victories...
            lastWinner = a
            
            If lastWinner = 0 Then
              WinsTable(id1, id2, 0) = WinsTable(id1, id2, 0) + 1
              WinsTable(id2, id1, 1) = WinsTable(id2, id1, 1) + 1
              SaveWinsTable
            Else
              WinsTable(id2, id1, 0) = WinsTable(id2, id1, 0) + 1
              WinsTable(id1, id2, 1) = WinsTable(id1, id2, 1) + 1
              SaveWinsTable
            End If
          
          End If
        Next a
        If lastWinner = -1 Then
          'Debug.Print "No winner yet. Starting round " & (r + 1) & "..."
          winner = -1
          PrepareRound r
          #If VAMPIRESTYLE = 0 Then
          sign = 2
          signtimer = 25
          players(0).Anim = FindAnimByName("stdNormal", players(0))
          players(1).Anim = FindAnimByName("stdNormal", players(1))
          #Else
          PlayFSBSound AnnouncerFSB, ANN_07_HEYCOMEONSTANDUP
          sign = 6
          signtimer = 25
          'TimeLeft =
          players(0).Locked = False
          players(1).Locked = False
          players(0).Anim = FindAnimByName("stdNormal", players(0))
          players(1).Anim = FindAnimByName("stdNormal", players(1))
          #End If
        Else
          'Debug.Print players(winner).FriendlyName & " wins the match."
          WipeOut
          VictoryTauntLoop winner
          Exit Sub
        End If
      End If
    End If
        
    QBltFast CameraX, 100 - slide, HealthBars.DDS, HealthBars.r, DDBLTFAST_SRCCOLORKEY
    QDrawBox CameraX + 182, 100 + 13 - slide, CameraX + 182 - (players(0).Health * 1.5), 100 + 22 - slide, QBColor(2), QBColor(2)
    QDrawBox CameraX + 202, 100 + 13 - slide, CameraX + 202 + (players(1).Health * 1.5), 100 + 22 - slide, QBColor(2), QBColor(2)
    QDrawBox CameraX + 182, 100 + 14 - slide, CameraX + 182 - (players(0).Health * 1.5), 100 + 20 - slide, QBColor(10), QBColor(10)
    QDrawBox CameraX + 202, 100 + 14 - slide, CameraX + 202 + (players(1).Health * 1.5), 100 + 20 - slide, QBColor(10), QBColor(10)
    For a = 0 To 1
      r2.Top = 0
      r2.Bottom = r2.Top + 8
      r2.Left = wintypes(0, a) * 8
      r2.Right = r2.Left + 8
      QBltFast CameraX + 38 + (a * 9), 102 - slide, WinIcons.DDS, r2, DDBLTFAST_SRCCOLORKEY
    Next a
    For a = 0 To 1
      r2.Top = 0
      r2.Bottom = r2.Top + 8
      r2.Left = wintypes(1, a) * 8
      r2.Right = r2.Left + 8
      QBltFast CameraX + 338 - (a * 9), 102 - slide, WinIcons.DDS, r2, DDBLTFAST_SRCCOLORKEY
    Next a
    
    Dim myBltFx As DDBLTFX
    myBltFx.lDDFX = DDBLTFX_MIRRORLEFTRIGHT
    r3.Left = CameraX + 348
    r3.Top = 100 + 3 - slide
    r3.Right = r3.Left + BarPortraits(0).r.Right
    r3.Bottom = r3.Top + BarPortraits(0).r.Bottom
    QBltFast CameraX + 5, 100 + 3 - slide, BarPortraits(0).DDS, BarPortraits(0).r, DDBLTFAST_SRCCOLORKEY
    QBltFx r3, BarPortraits(1).DDS, BarPortraits(1).r, DDBLT_KEYSRC + DDBLT_DDFX, myBltFx
    
    DDS_back.SetFillStyle 1
    If TimeLeft = -1 Then
      DrawString CameraX + 192 - 4, 109 - slide + 4, Chr(149), False, 3
    Else
      DrawString CameraX + 192 - 8, 109 - slide, Format(TimeLeft, "00"), True, IIf((TimeLeft < 10) And (framecount Mod 10 < 5), 3, 0)
    End If
    DrawString CameraX + 54, 100 + 26 - slide, players(0).FriendlyName
    DrawString CameraX + 329 - (Len(players(1).FriendlyName) * 8), 100 + 26 - slide, players(1).FriendlyName
    
    If sign > 0 And sign < 3 Then
      If players(0).Health < 100 Then players(0).Health = players(0).Health + 3
      If players(1).Health < 100 Then players(1).Health = players(1).Health + 3
      'QBltFast CameraX + 192 - (Signs(sign - 1).r.Right / 2), 100 + 60 - (Signs(sign - 1).r.Bottom / 2), Signs(sign - 1).ddS, Signs(sign - 1).r, DDBLTFAST_SRCCOLORKEY
      r2.Left = 0
      r2.Right = 220
      r2.Top = (sign - 1) * 52
      r2.Bottom = r2.Top + 52
      QBltFast CameraX + 192 - 110, 100 + 60 - 26, Signs.DDS, r2, DDBLTFAST_SRCCOLORKEY
      
      signtimer = signtimer - 1
      If signtimer = 0 Then
        signtimer = 10
        sign = sign - 1
        If sign = 1 Then PlayFSBSound AnnouncerFSB, ANN_01_PREPAREFORBATTLE + Int(Rnd * (ANN_03_READYSET))
        If sign = 0 Then
          GeneralStats.RoundsPlayed = GeneralStats.RoundsPlayed + 1
          SaveWinsTable
          
          players(0).Anim = 0
          players(0).Frame = 0
          players(1).Anim = 0
          players(1).Frame = 0
          players(0).Locked = False
          players(1).Locked = False
        End If
      End If
    End If
    If sign > 3 Then
      If sign = 4 Then 'You Win or Perfect
        If winner < 0 Then 'Oops, can't have this sign without a winner...
          sign = 0
        Else
          If LastSuper <> "" Then
            DrawString CameraX + 192 - ((Len(LastSuper) * 8) / 2), 200, LastSuper, True, 5
          End If
          If players(winner).Health = 100 Then
            'QBltFast CameraX + 192 - (Signs(3).r.Right / 2), 100 + 60 - (Signs(3).r.Bottom / 2), Signs(3).ddS, Signs(3).r, DDBLTFAST_SRCCOLORKEY
          Else
            'QBltFast CameraX + 192 - (Signs(4).r.Right / 2), 100 + 60 - (Signs(4).r.Bottom / 2), Signs(4).ddS, Signs(4).r, DDBLTFAST_SRCCOLORKEY
          End If
        End If
      End If
      If sign = 5 Then 'Time Over
        'QBltFast CameraX + 192 - (Signs(2).r.Right / 2), 100 + 60 - (Signs(2).r.Bottom / 2), Signs(2).ddS, Signs(2).r, DDBLTFAST_SRCCOLORKEY
      End If
      If sign = 6 Then 'Get Up
        'QBltFast CameraX + 192 - (Signs(5).r.Right / 2), 100 + 60 - (Signs(5).r.Bottom / 2), Signs(5).ddS, Signs(2).r, DDBLTFAST_SRCCOLORKEY
        signtimer = signtimer - 1
      End If
      If sign = 7 Then 'You Win
        If winner < 0 Then 'Oops, can't have this sign without a winner...
          sign = 0
        Else
          'QBltFast CameraX + 192 - (Signs(4).r.Right / 2), 100 + 60 - (Signs(4).r.Bottom / 2), Signs(4).ddS, Signs(4).r, DDBLTFAST_SRCCOLORKEY
        End If
      End If
      If signtimer = 0 Then sign = 0
    End If
    
    If ShowDebugInfo Then
      DrawString CameraX + 10, 140, "Anim: " & players(0).Anim & " - " & players(0).Anims(players(0).Anim).AnimName, True
      DrawString CameraX + 10, 155, "Frame: " & players(0).Frame & "/" & players(0).Anims(players(0).Anim).FrameC, True
      'DrawString CameraX + 10, 170, "Song: " & (FSOUND_Stream_GetTime(streamHandle) / 1000) & " s", True
      'DrawString CameraX + 10, 185, "Keys: " & players(0).LastKeys, True
      'DrawString CameraX + 10, 200, "KT: " & players(0).LastKeysTimer, True
      
      DrawString CameraX + 10, 200, "Recovery: " & players(0).RecoveryDelay, True
    End If
    
    DrawString CameraX + 10, 292, "KAFÉ", True, 3
    DrawString CameraX + 10, 308, "Kawa's Fighting Game Attempt #3", False, 3
    
    FlipIt
    QFlush
  Wend
FinishLoop:
  WipeOut
End Sub

'Select a Fighter screen. Sets id1 and id2 to the chosen fighter indices.
Public Sub SelectLoop()
  Dim r2 As RECT, r3 As RECT, b As Integer, c As Integer
  Dim a As Long, zzz As Long, timer As Long
  Dim bullshit As String, bullshitscroller As Long
  Dim sel(MAXPLAYERS) As Integer
  Dim done(MAXPLAYERS) As Integer
  Dim ports(MAXFIGHTERS) As tSurface
  Dim lilports As tSurface
  Dim sback As tSurface
  Dim myBltFx As DDBLTFX
  Dim replock(MAXPLAYERS) As Integer, chosen(MAXPLAYERS) As Integer
  Dim s As String
  Background = backgrounds(1)
  CameraX = 96
  
  If StopIt = True Then End
  
  myBltFx.lDDFX = DDBLTFX_MIRRORLEFTRIGHT
  
  For a = 0 To fightersinpool - 1
    s = LoadFile(fighterpool(a).FilenameBase & "_sel.bmp")
    LoadSurface ports(a), s, True
    KillFile s
  Next a
  s = LoadFile("select.bmp")
  LoadSurface lilports, s
  KillFile s
  s = LoadFile("selback.bmp")
  LoadSurface sback, s
  KillFile s
  
  PlaySong "utemple", True
  PlayFSBSound AnnouncerFSB, ANN_17_SELCHAR
  timer = 20

  For a = 0 To MAXPLAYERS
    sel(a) = a
    players(a) = fighterpool(sel(a))
    players(a).Frame = 0
    players(a).Anim = 0
  Next a
    
  bullshitscroller = 384
  bullshit = "Forget all you learned about everything. Focus on the inevitable. Keep your eyes on your opponent. Please don't read this text. It all depends on your skill. Forget danger, you're not safe anyway. Watch out for snakes. If you can read this, you don't need glasses. Can you fight your way to glory? Born to fight..."
  
  While Not StopIt
    HandleTimingAndInput
    If framecount Mod 16 = 1 Then timer = timer - 1
    If timer = 0 Then
      id1 = sel(0)
      players(0) = fighterpool(id1)
      players(0).Controller = fcPlayer1
      If NumPlayers = 1 Then
        id2 = sel(1)
        players(1) = fighterpool(id2)
        players(1).Controller = fcPlayer2
      End If
      done(0) = 1
      done(1) = 1
    End If

    DrawBackground
        
    'DrawString CameraX + bullshitscroller, 100, bullshit, False, 1
    DrawString CameraX + bullshitscroller, 314, bullshit, False, 1
    bullshitscroller = bullshitscroller - 8
    If bullshitscroller = -(Len(bullshit) * 8) Then bullshitscroller = 384
        
    DrawString CameraX + 110, 104, "Select your Character", True
    
    For c = 0 To NumPlayers
      For a = -6 To 7
        b = a + sel(c) - 2
        b = b Mod fightersinpool
        'If b >= fightersinpool Then b = b - fightersinpool
        If b < 0 Then b = fightersinpool + b
        r2.Top = 0
        r2.Bottom = r2.Top + 48
        r2.Left = (b * 48) + 96
        r2.Right = r2.Left + 48
        QBltFast CameraX + 72 + (a * 48) + replock(c), 150 + (c * 48), lilports.DDS, r2, DDBLTFAST_SRCCOLORKEY
        'DrawString CameraX + 72 + (a * 48), 150 + (c * 48), CStr(b)
      Next a
      If (done(c) = 0) Or (framecount Mod 2 = 1) Then
        r2.Top = 0
        r2.Bottom = r2.Top + 48
        r2.Left = c * 48
        r2.Right = r2.Left + 48
        QBltFast CameraX + 168, 150 + (c * 48), lilports.DDS, r2, DDBLTFAST_SRCCOLORKEY
      End If
    Next c
    
    r2.Top = 101
    r2.Left = CameraX + 3
    r2.Bottom = r2.Top + 192
    r2.Right = r2.Left + 98
    QBltFast CameraX + 2, 100, sback.DDS, sback.r, DDBLTFAST_NOCOLORKEY
    QBlt r2, ports(sel(0)).DDS, ports(sel(0)).r, DDBLT_KEYSRC
    
    If NumPlayers = 1 Then
      r2.Top = 101
      r2.Left = CameraX + 283
      r2.Bottom = r2.Top + 192
      r2.Right = r2.Left + 98
      QBltFast CameraX + 282, 100, sback.DDS, sback.r, DDBLTFAST_NOCOLORKEY
      QBltFx r2, ports(sel(1)).DDS, ports(sel(0)).r, DDBLT_KEYSRC + DDBLT_DDFX, myBltFx
    End If
    
    For a = 0 To NumPlayers
      With fighterpool(sel(a))
        .X = IIf(a = 0, CameraX + 64, CameraX + 384 - 64)
        .Y = GROUND + 8
        .Frame = .Frame + 1
        .FacingLeft = (a = 1)
        If .Frame = .Anims(.Anim).FrameC Then .Frame = 0
        With .Anims(.Anim).Frames(.Frame)
          r2.Left = 0
          r2.Top = 8
          r2.Right = r2.Left + 64
          r2.Bottom = r2.Top + 8
          r3.Top = GROUND - 4 + 8
          r3.Bottom = r3.Top + 8
          r3.Left = fighterpool(sel(a)).X - 32
          r3.Right = r3.Left + 64
          QCardboard r3, ParticleSurf.DDS, r2, 0.5, 0
        End With
        DrawFighter fighterpool(sel(a)), False
      End With
      If a = 0 Then
        DrawString CameraX + 8, 300, fighterpool(sel(a)).FriendlyName, True
      Else
        DrawString CameraX + 384 - 8 - (Len(fighterpool(sel(a)).FriendlyName) * 8), 300, fighterpool(sel(a)).FriendlyName, True
      End If
    Next a
    
    DrawString 150, 120, "Frame: " & players(0).Frame & "/" & players(0).Anims(players(0).Anim).FrameC, True
    
    'For a = 0 To fightersinpool - 1
    '  DrawString 150, 100 + 20 + (a * 10), fighterpool(a).FriendlyName, False
    'Next a
    
    zzz = 0
    If Keypad(1).bStart And NumPlayers = 0 Then
      'TODO: Add sound and video feedback
      NumPlayers = 1
      timer = timer + 15
    End If
    
    For a = 0 To NumPlayers
      If done(a) = 0 Then
        If replock(a) = 0 Then
          If Keypad(a).bRight Then
            sel(a) = sel(a) + 1
            replock(a) = 48
          End If
          If Keypad(a).bLeft Then
            sel(a) = sel(a) - 1
            replock(a) = -48
          End If
          If sel(a) < 0 Then sel(a) = fightersinpool - 1
          If sel(a) > fightersinpool - 1 Then sel(a) = 0
          players(a) = fighterpool(sel(a))
        Else
          If replock(a) < 0 Then
            replock(a) = replock(a) + 8
          Else
            replock(a) = replock(a) - 8
          End If
        End If
        
        If Keypad(a).bOneKick Then
          WantedRecolor(a) = 1
          Keypad(a).bOnePunch = True
        End If
        If Keypad(a).bOnePunch Then
          If a = 0 Then
            id1 = sel(a)
            players(0) = fighterpool(id1)
            players(0).Controller = fcPlayer1
          Else
            id2 = sel(a)
            players(1) = fighterpool(id2)
            players(1).Controller = fcPlayer2
          End If
          done(a) = 1
          chosen(a) = 50
          'TODO: Say my name BITCH!
          FSBHandles(a) = FSOUND_Stream_Open(players(a).FilenameBase & ".fsb", FSOUND_NONBLOCKING, 0, 0)
          PlayFSBSound FSBHandles(a), players(a).FSBLongName, 128
        End If
      End If
      
      If chosen(a) > 0 Then chosen(a) = chosen(a) - 1
      
      zzz = zzz + done(a)
      If zzz > NumPlayers And chosen(0) = 0 And chosen(1) = 0 Then
        WipeOut
        Exit Sub
      End If
    Next a
    
    DrawString CameraX + 305, 108, "Time left", False
    DrawString CameraX + 360, 115, Format(timer, "00"), True, IIf((timer < 10) And (framecount Mod 10 < 5), 3, 0)
    
    If framecount Mod 6 > 2 And NumPlayers = 0 Then DrawString CameraX + 325, 102, "JOIN IN", False
    
    FlipIt
    QFlush
  Wend
End Sub

'Foo Versus Baz screen. Shows who's up next.
Public Sub VersusLoop()
  Dim r2 As RECT, r3 As RECT
  Dim a As Long, z1 As Integer, z2 As Integer, streamLen As Long, s(5) As String
  Dim ports(2) As tSurface
  Dim myBltFx As DDBLTFX
  
  If StopIt = True Then End
  
  myBltFx.lDDFX = DDBLTFX_MIRRORLEFTRIGHT
  Background = backgrounds(8)
  s(0) = LoadFile(players(0).FilenameBase & "_sel.bmp")
  s(1) = LoadFile(players(1).FilenameBase & "_sel.bmp")
  LoadSurface ports(0), s(0), True, players(0).Recolor
  LoadSurface ports(1), s(1), True, players(1).Recolor
  KillFile s(0)
  KillFile s(1)
  
  z1 = 400
  z2 = 400
  framecount = 0
  While Not StopIt
    HandleTimingAndInput
    
    If z1 > 0 Then z1 = z1 - 50
    If z1 = 0 And z2 = 400 Then z2 = 0
    If z1 = 0 And z2 < 254 Then z2 = z2 + 50
    If z1 = 0 And z2 > 254 Then z2 = 254
    
    DrawBackground
    
    If z1 = 0 Then DrawString CameraX + 192 - ((Len(players(0).FriendlyName) * 8) / 2), 180, players(0).FriendlyName, True
    DrawString CameraX + 192 - ((Len("versus") * 8) / 2), 204, "versus", False
    If z2 = 254 Then DrawString CameraX + 192 - ((Len(players(1).FriendlyName) * 8) / 2), 218, players(1).FriendlyName, True
    
    r2.Top = 100
    r2.Left = CameraX + z2
    r2.Bottom = r2.Top + 224
    r2.Right = r2.Left + 130
    QBltFx r2, ports(1).DDS, ports(1).r, DDBLT_KEYSRC + DDBLT_DDFX, myBltFx
    
    r2.Top = 100
    r2.Left = CameraX + z1
    r2.Bottom = r2.Top + 224
    r2.Right = r2.Left + 130
    QBlt r2, ports(0).DDS, ports(0).r, DDBLT_KEYSRC
    
    'DrawString CameraX + 5, 100, FSOUND_Stream_GetTime(streamHandle) & " / " & streamLen, True
    If framecount > 100 Then GoTo FinishLoop
    
    FlipIt
    QFlush
  Wend
FinishLoop:
  WipeOut
End Sub

'Given the winner's index, shows that fighter's Win Picture and a randomly-picked taunt.
Public Sub VictoryTauntLoop(fighter As Integer)
  Dim r2 As RECT, r3 As RECT
  Dim a As Long, z As Integer, streamLen As Long, t As Integer, s As String
  Dim fighterPic As tSurface
  
  If StopIt = True Then End
  Do
    t = Int(Rnd * MAXTAUNTS)
  Loop Until players(fighter).Taunts(t) <> ""
  
  Background = backgrounds(2)
  s = LoadFile(players(fighter).FilenameBase & "_win.bmp")
  LoadSurface fighterPic, s, True
  KillFile s
  
  z = 400
  PlaySong players(fighter).FilenameBase & ", chosen", False
  'streamLen = FSOUND_Stream_GetLengthMs(streamHandle)
  framecount = 0
  While Not StopIt
    HandleTimingAndInput
    
    If z > 0 Then z = z - 50
    
    DrawBackground
    
    QBltFast CameraX + z, 100, fighterPic.DDS, fighterPic.r, DDBLTFAST_SRCCOLORKEY
    DrawString CameraX + 10, 284, players(fighter).Taunts(t), True
    
    'DrawString 5, 100, FSOUND_Stream_GetTime(streamHandle) & " / " & streamLen, True
    'If FSOUND_Stream_GetTime(streamHandle) >= streamLen - 50 Or framecount > 160 Or Keypad(0).b1 Then
    If framecount > 160 Or Keypad(0).b1 Then
      WipeOut
      Exit Sub
    End If
    
    FlipIt
    QFlush
  Wend
End Sub

Public Sub StoryLoop(fighter As Integer, storynum As Integer)
  Dim r2 As RECT, r3 As RECT
  Dim a As Long, z As Integer, t As Integer, p As Integer, s As String
  Dim pic As tSurface
  
  If StopIt = True Then End
  
  If Stories(fighter, storynum).NumPars = 0 Then
    'Debug.Print "StoryLoop: " & fighterpool(fighter).FriendlyName & " has no story."
    Exit Sub
  End If
  
  Background = backgrounds(2)
  If Stories(fighter, storynum).Paragraphs(p).Image <> "" Then
    s = LoadFile(Stories(fighter, storynum).Paragraphs(p).Image)
    LoadSurface pic, s, True
    KillFile s
  Else
    s = LoadFile(players(fighter).FilenameBase & "_win.bmp")
    LoadSurface pic, s, True
    KillFile s
  End If
  z = 400
  PlaySong Stories(fighter, storynum).Music
  framecount = 0
  While Not StopIt
    HandleTimingAndInput
    
    DrawBackground
    
    QBltFast CameraX, 100, pic.DDS, pic.r, DDBLTFAST_SRCCOLORKEY
    DrawString CameraX + 10, 268, Stories(fighter, storynum).Paragraphs(p).Text, True
    
    If framecount > 2 Then
      If Keypad(0).b1 Or framecount > 160 Then
        framecount = 0
        p = p + 1
        If p = Stories(fighter, storynum).NumPars Then GoTo StopLoop
        If Stories(fighter, storynum).Paragraphs(p).Image <> "" Then
          s = LoadFile(Stories(fighter, storynum).Paragraphs(p).Image)
          LoadSurface pic, s, True
          KillFile s
        End If
      End If
    End If
    
    'DrawString CameraX + 4, 104, CStr(framecount), True
    
    FlipIt
    QFlush
  Wend
StopLoop:
  WipeOut
End Sub

Public Sub TitleLoop()
  Dim r2 As RECT, r3 As RECT, tits As tSurface, s As String
  Dim myBltFx As DDBLTFX
  
  If StopIt = True Then End
  
  Background = backgrounds(3)
  PlaySong "utemple"
  CameraX = 0
  s = LoadFile("title.bmp")
  LoadSurface tits, s, True, 0
  KillFile s
  
  While Not StopIt
    HandleTimingAndInput
    
    DrawBackground
    
    If framecount Mod 6 < 3 Then DrawString CameraX + 3, 102, "PRESS START", False
    If framecount Mod 6 > 2 Then DrawString CameraX + 293, 102, "PLEASE WAIT", False
    
    QBltFast CameraX, 100, tits.DDS, tits.r, DDBLTFAST_SRCCOLORKEY
    'DrawString CameraX + 100 + (Sin(framecount / 20) * 55), 170 + (Cos(framecount / 40) * 25), "Insert title screen here.", True, 1
    'DrawString CameraX + 100 + (Sin(framecount / 20) * 50), 170 + (Cos(framecount / 40) * 20), "Insert title screen here.", True, 5
    
    If framecount Mod 24 > 12 Then DrawString CameraX + 80, 278, "(c) 2006 The Helmeted Rodent", False
    #If VAMPIRESTYLE = 1 Then
    DrawString CameraX + 10, 306, "NOTE: Darkstalker mode is now on. On defeat, you'll continue" & vbCrLf & "from your previous time and place.", False, 6
    #End If
    
    If Keypad(0).bStart Or Keypad(0).b1 Then GoTo FinishLoop
    
    FlipIt
    QFlush
  Wend
FinishLoop:
  WipeOut wipeTV
End Sub

Public Sub RosterLoop()
  Dim r2 As RECT, r3 As RECT, i As Integer, a As Integer, tower As tSurface, z As Integer
  Dim myBltFx As DDBLTFX, properstart As Integer, properend As Integer, port As tSurface, s(5) As String
  
  If StopIt = True Then End
  
  
  Background = backgrounds(2)
  'streamHandle = PlaySong("eternal.ogg")
  framecount = 0
  
  s(0) = LoadFile("rostertower.bmp")
  s(1) = LoadFile(players(1).FilenameBase & "_sel.bmp")
  LoadSurface tower, s(0), True
  LoadSurface port, s(1), True, players(1).Recolor
  KillFile s(0)
  KillFile s(1)
  
  properstart = ANN_06_KEEPITUP
  properend = ANN_08_NOTURNINGBACK
  z = 400
  If WinsSoFar Then PlayFSBSound AnnouncerFSB, properstart + Int(Rnd * (properend - properstart))
  
  HereComesANewChallenger = False
  
  While Not StopIt
    HandleTimingAndInput
    If NumPlayers = 0 And Keypad(1).bStart Then
      NewChallengerLoop
      HereComesANewChallenger = True
      WipeOut
      Exit Sub
    End If
    
    If z Then z = z - 25

    DrawBackground
    
    r2.Top = 100
    r2.Left = CameraX + z
    r2.Bottom = r2.Top + 224
    r2.Right = r2.Left + 130
    QBltFx r2, port.DDS, port.r, DDBLTFAST_SRCCOLORKEY + DDBLT_DDFX, myBltFx
    
    'DrawString CameraX + 10, 110, "Next up", True
    
    r3.Top = 20
    r3.Left = 4
    r3.Bottom = 22
    r3.Right = 6
    r2.Left = CameraX
    r2.Right = CameraX + 384
    r2.Top = 120
    r2.Bottom = 240
    QCardboard r2, ParticleSurf.DDS, r3, 0.75
    
    With fighterpool(FindIndexByFriendlyName(RosterOpponents(id1, WinsSoFar).FriendlyName))
      DrawString CameraX + 150 - (z / 2), 125, .FullName, True
      i = 150
      If .HomeLand <> "" And .HomeArea <> "" Then
        DrawString CameraX + 150 - (z / 2), i, "Home: " & .HomeArea & ", " & .HomeLand, False
        i = i + 8
      ElseIf .HomeLand <> "" And .HomeArea = "" Then
        DrawString CameraX + 150 - (z / 2), i, "Home: " & .HomeLand, False
        i = i + 8
      ElseIf .HomeLand = "" And .HomeArea <> "" Then
        DrawString CameraX + 150 - (z / 2), i, "Home: " & .HomeArea, False
        i = i + 8
      End If
      If MetricSystem Then
        If .Height Then
          DrawString CameraX + 150 - (z / 2), i, "Height: " & (.Height / 100) & " meters", False
          i = i + 8
        End If
        If .Weight Then
          DrawString CameraX + 150 - (z / 2), i, "Weight: " & (.Weight / 100) & " kilos", False
          i = i + 8
        End If
      Else
        If .Height Then
          DrawString CameraX + 150 - (z / 2), i, "Height: " & Format(((.Height * 0.3937) / 10), "###.##") & " feet", False
          i = i + 8
        End If
        If .Weight Then
          DrawString CameraX + 150 - (z / 2), i, "Weight: " & Format(((.Weight * 2.20462) / 100), "###.##") & " lbs", False
          i = i + 8
        End If
      End If
    End With
    
    
    
    For i = 0 To NumOpponents
      r2.Top = 0
      r2.Bottom = r2.Top + 20
      r2.Left = 0
      r2.Right = r2.Left + 112
      QBltFast CameraX + 384 - 120, 304 - (NumOpponents * 20) + (i * 20), tower.DDS, r2, DDBLTFAST_SRCCOLORKEY
    Next i
    For i = 0 To NumOpponents
      If i <= WinsSoFar Then
        DrawString CameraX + 384 - 120 + 8, 308 - (i * 20), fighterpool(FindIndexByFriendlyName(RosterOpponents(id1, i).FriendlyName)).FriendlyName, False, 1
      Else
        DrawString CameraX + 384 - 120 + 8, 308 - (i * 20), "?", False, 1
      End If
    Next i
    r2.Top = 20
    r2.Bottom = r2.Top + 16
    If framecount Mod 2 = 0 Then QBltFast CameraX + 384 - 120, 304 - (WinsSoFar * 20), tower.DDS, r2, DDBLTFAST_SRCCOLORKEY
    'DrawString CameraX + 20, 130 + (WinsSoFar * 10), ">", False
    
'    If RosterOpponents(id1, WinsSoFar).BackgroundOverrule Then
'      a = RosterOpponents(id1, WinsSoFar).BackgroundOverrule
'    Else
'      a = fighterpool(FindIndexByFriendlyName(RosterOpponents(id1, WinsSoFar).FriendlyName)).HomeArena
'    End If
'    DrawString CameraX + 20, 300, "Fight in " & backgrounds(a).FriendlyName & ".", False
    
    If framecount > 100 Then GoTo FinishLoop
    
    FlipIt
    QFlush
  Wend
FinishLoop:
  WipeOut
End Sub

Public Sub BlankLoop()
  Dim r2 As RECT, r3 As RECT, a As Single, b As Integer
  Dim myBltFx As DDBLTFX
  
  If StopIt = True Then End
  
  Background = backgrounds(1)
  'PlaySong "rain", True
  While Not StopIt
    HandleTimingAndInput
    
    DrawBackground
    
    DrawString CameraX + 10, 110, "Insert screen code here.", True
    
    r2.Top = 0
    r2.Left = 64
    r2.Bottom = 64
    r2.Right = 128
    r3.Left = 64
    r3.Top = 100 + 64
    r3.Right = r3.Left + (64 * (2 + Sin(framecount / 10)))
    r3.Bottom = r3.Top + (64 * (2 + Sin(framecount / 5)))
    
    For b = 1 To 16
      QCardboard r3, ParticleSurf.DDS, r2, 0.0625 * b, (a + (b * 8)) * (3.14 / 180), 1
    Next b
    
    a = (a + 4) Mod 359
    
    FlipIt
    QFlush
  Wend
FinishLoop:
  WipeOut
End Sub

Public Function EnterNameLoop()
  Dim i As Integer, s As String, SoFar As String
  If StopIt = True Then End
  
  Background = backgrounds(2)
  While Not StopIt
    HandleTimingAndInput
    
    s = ""
    If newkeyboard(DIK_RETURN) = 3 And Len(SoFar) Then GoTo FinishLoop
    If newkeyboard(DIK_BACK) = 3 And Len(SoFar) Then SoFar = Left(SoFar, Len(SoFar) - 1)
    If newkeyboard(DIK_SPACE) = 3 Then s = " "
    If newkeyboard(DIK_COMMA) = 3 Then s = ","
    If newkeyboard(DIK_PERIOD) = 3 Then s = "."
    If newkeyboard(DIK_A) = 3 Then s = "a"
    If newkeyboard(DIK_B) = 3 Then s = "b"
    If newkeyboard(DIK_C) = 3 Then s = "c"
    If newkeyboard(DIK_D) = 3 Then s = "d"
    If newkeyboard(DIK_E) = 3 Then s = "e"
    If newkeyboard(DIK_F) = 3 Then s = "f"
    If newkeyboard(DIK_G) = 3 Then s = "g"
    If newkeyboard(DIK_H) = 3 Then s = "h"
    If newkeyboard(DIK_I) = 3 Then s = "i"
    If newkeyboard(DIK_J) = 3 Then s = "j"
    If newkeyboard(DIK_K) = 3 Then s = "k"
    If newkeyboard(DIK_L) = 3 Then s = "l"
    If newkeyboard(DIK_M) = 3 Then s = "m"
    If newkeyboard(DIK_N) = 3 Then s = "n"
    If newkeyboard(DIK_O) = 3 Then s = "o"
    If newkeyboard(DIK_P) = 3 Then s = "p"
    If newkeyboard(DIK_Q) = 3 Then s = "q"
    If newkeyboard(DIK_R) = 3 Then s = "r"
    If newkeyboard(DIK_S) = 3 Then s = "s"
    If newkeyboard(DIK_T) = 3 Then s = "t"
    If newkeyboard(DIK_U) = 3 Then s = "u"
    If newkeyboard(DIK_V) = 3 Then s = "v"
    If newkeyboard(DIK_W) = 3 Then s = "w"
    If newkeyboard(DIK_X) = 3 Then s = "x"
    If newkeyboard(DIK_Y) = 3 Then s = "y"
    If newkeyboard(DIK_Z) = 3 Then s = "z"
    If newkeyboard(DIK_LSHIFT) Or newkeyboard(DIK_RSHIFT) Then
      s = UCase(s)
      If newkeyboard(DIK_APOSTROPHE) = 3 Then s = """"
      If newkeyboard(DIK_GRAVE) = 3 Then s = "~"
      If newkeyboard(DIK_0) = 3 Then s = ")"
      If newkeyboard(DIK_1) = 3 Then s = "!"
      If newkeyboard(DIK_2) = 3 Then s = "@"
      If newkeyboard(DIK_3) = 3 Then s = "#"
      If newkeyboard(DIK_4) = 3 Then s = "$"
      If newkeyboard(DIK_5) = 3 Then s = "%"
      If newkeyboard(DIK_6) = 3 Then s = "^"
      If newkeyboard(DIK_7) = 3 Then s = "&"
      If newkeyboard(DIK_8) = 3 Then s = "*"
      If newkeyboard(DIK_9) = 3 Then s = "("
    Else
      If newkeyboard(DIK_APOSTROPHE) = 3 Then s = "'"
      If newkeyboard(DIK_GRAVE) = 3 Then s = "`"
      If newkeyboard(DIK_0) = 3 Then s = "0"
      If newkeyboard(DIK_1) = 3 Then s = "1"
      If newkeyboard(DIK_2) = 3 Then s = "2"
      If newkeyboard(DIK_3) = 3 Then s = "3"
      If newkeyboard(DIK_4) = 3 Then s = "4"
      If newkeyboard(DIK_5) = 3 Then s = "5"
      If newkeyboard(DIK_6) = 3 Then s = "6"
      If newkeyboard(DIK_7) = 3 Then s = "7"
      If newkeyboard(DIK_8) = 3 Then s = "8"
      If newkeyboard(DIK_9) = 3 Then s = "9"
    End If
    SoFar = SoFar & s
    If Right(SoFar, 2) = "'A" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Á"
    If Right(SoFar, 2) = "`A" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "À"
    If Right(SoFar, 2) = "~A" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ã"
    If Right(SoFar, 2) = "^A" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Â"
    If Right(SoFar, 2) = """A" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ä"
    If Right(SoFar, 2) = "'a" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "á"
    If Right(SoFar, 2) = "`a" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "à"
    If Right(SoFar, 2) = "~a" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "à"
    If Right(SoFar, 2) = "^a" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "â"
    If Right(SoFar, 2) = """a" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ä"
    If Right(SoFar, 2) = "'E" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "É"
    If Right(SoFar, 2) = "`E" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "È"
    If Right(SoFar, 2) = "^E" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ê"
    If Right(SoFar, 2) = """E" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ë"
    If Right(SoFar, 2) = "'e" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "é"
    If Right(SoFar, 2) = "`e" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "è"
    If Right(SoFar, 2) = "^e" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ê"
    If Right(SoFar, 2) = """e" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ë"
    If Right(SoFar, 2) = "'I" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Í"
    If Right(SoFar, 2) = "`I" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ì"
    If Right(SoFar, 2) = "^I" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Î"
    If Right(SoFar, 2) = """I" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ï"
    If Right(SoFar, 2) = "'i" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "í"
    If Right(SoFar, 2) = "`i" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ì"
    If Right(SoFar, 2) = "^i" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "î"
    If Right(SoFar, 2) = """i" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ï"
    If Right(SoFar, 2) = "'O" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ó"
    If Right(SoFar, 2) = "`O" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ò"
    If Right(SoFar, 2) = "^O" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ô"
    If Right(SoFar, 2) = "~O" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Õ"
    If Right(SoFar, 2) = """O" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ö"
    If Right(SoFar, 2) = "'o" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ó"
    If Right(SoFar, 2) = "`o" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ò"
    If Right(SoFar, 2) = "~o" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "õ"
    If Right(SoFar, 2) = "^o" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ô"
    If Right(SoFar, 2) = """o" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ö"
    If Right(SoFar, 2) = "~N" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ñ"
    If Right(SoFar, 2) = "~n" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ñ"
    If Right(SoFar, 2) = "'U" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ú"
    If Right(SoFar, 2) = "`U" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ù"
    If Right(SoFar, 2) = "^U" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Û"
    If Right(SoFar, 2) = """U" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "Ü"
    If Right(SoFar, 2) = "'u" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "é"
    If Right(SoFar, 2) = "`u" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ù"
    If Right(SoFar, 2) = "^u" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "û"
    If Right(SoFar, 2) = """u" Then SoFar = Left(SoFar, Len(SoFar) - 2) & "ü"
    If Right(SoFar, 3) = "(C)" Then SoFar = Left(SoFar, Len(SoFar) - 3) & "©"
    If Right(SoFar, 3) = "(c)" Then SoFar = Left(SoFar, Len(SoFar) - 3) & "©"
    
    DrawBackground
    
    DrawString CameraX + 10, 110, "High score!", True
    DrawString CameraX + 10, 130, "Enter your name:", False
    DrawString CameraX + 10, 150, SoFar & IIf(framecount Mod 8 < 4, "_", ""), True, 5
        
    FlipIt
    QFlush
  Wend
FinishLoop:
  EnterNameLoop = SoFar
End Function

Public Sub BrandingLoop()
  Dim Logo As tSurface, a As Integer, r2 As RECT, r3 As RECT, b As Integer, c As Single, s As String
  
  If StopIt = True Then End
  s = LoadFile("kafelogo.bmp")
  LoadSurface Logo, s, False, 0
  c = 20
  
  While Not StopIt
    HandleTimingAndInput
    
    QDrawBox 0, 100, 600, 324, &HFFFFFF, &HFFFFFF
    CameraX = 50
    
    For a = 0 To 223 Step 1
      r2.Left = 0
      r2.Right = 386
      r2.Top = a - (Sin((a + b) / 10) * c)
      r2.Bottom = r2.Top + 1
      r3.Left = CameraX + (Sin((a + b) / 15) * (c + 0.5))
      r3.Right = r3.Left + 386
      r3.Top = a + 100
      r3.Bottom = r3.Top + 1 + c
      QBlt r3, Logo.DDS, r2, DDBLT_DONOTWAIT
    Next a
    b = b + 1
    If c > 0 And b Mod 2 = 1 Then c = c - 0.5
    If c = 0 And b Mod 50 = 1 Then GoTo FinishLoop
    If Keypad(0).b1 Then GoTo FinishLoop
    'DrawString CameraX + 10, 110, "Insert screen code here.", True
    
    FlipIt
    QFlush
  Wend
FinishLoop:
  KillFile s
  WipeOut
End Sub


Public Sub SelectJoypads()
  Dim r2 As RECT, r3 As RECT
  Dim a As Long, timer As Long, b As Integer
  Dim sel(MAXPLAYERS) As Integer
  Dim done(MAXPLAYERS) As Integer
  Dim replock(MAXPLAYERS) As Integer
  Background = backgrounds(8)
  
  If StopIt = True Then End
    
  PlaySong "utemple", True
  'PlayFSBSound AnnouncerFSB, ANN_23_SELECTYOURCHARACTER
  timer = 30
  sel(0) = playerpads(0)
  sel(1) = playerpads(1)
  
  While Not StopIt
    HandleTimingAndInput
    If framecount Mod 16 = 1 Then timer = timer - 1
    If timer = 0 Then
      done(0) = 1
      done(1) = 1
    End If
    
    DrawBackground
    
    DrawString 305, 108, "Time left", False
    DrawString 360, 115, Format(timer, "00"), True
    
    DrawString 20, 100 + 20, "Select your Gamepads", True
    DrawString 20, 100 + 40, "You have one or more joypads or similar" & vbCrLf & "input devices connected. Both players get" & vbCrLf & "to choose which input device to use.", False
    
    r2.Left = 50
    r2.Top = 18
    r2.Right = 52
    r2.Bottom = 20
    For a = 0 To 1
      For b = 0 To 5
        r3.Left = 12 + b
        r3.Right = 200 - b
        r3.Top = 189 + (sel(a) * 10)
        r3.Bottom = r3.Top + 10
        QCardboard r3, ParticleSurf.DDS, r2, 0.125 + (Sin(framecount + IIf(a = 1, 1, 0)) / 10)
      Next b
    Next a
    
    DrawString 40, 100 + 80, "Just the keyboard", False
    For a = 0 To numpads - 1
      DrawString 40, 190 + (a * 10), didevenum.GetItem(a + 1).GetProductName, False
    Next a
    If sel(0) = sel(1) Then
      DrawString 12, 190 + (sel(0) * 10), "12>", False, 3
    Else
      If (done(0) = 0) Or (framecount Mod 2 = 1) Then DrawString 20, 190 + (sel(0) * 10), "1>", False, 3
      If (done(1) = 0) Or (framecount Mod 2 = 0) Then DrawString 20, 190 + (sel(1) * 10), "2>", False, 3
    End If
    
    For a = 0 To 1
      If replock(a) > 0 Then replock(a) = replock(a) - 1
      If Keypad(a).bDown And replock(a) = 0 Then
        sel(a) = IIf(sel(a) < numpads - 1, sel(a) + 1, -1)
        replock(a) = 5
      End If
      If Keypad(a).bUp And replock(a) = 0 Then
        sel(a) = IIf(sel(a) > -1, sel(a) - 1, numpads - 1)
        replock(a) = 5
      End If
      If Keypad(a).b1 Then
        playerpads(a) = sel(a)
        done(a) = 1
      End If
    Next a
    If done(0) And done(1) Then
      WipeOut
      Exit Sub
    End If
    
    FlipIt
    QFlush
  Wend
End Sub

Public Sub WipeOut(Optional effect As eWipes = -1)
  Dim i As Integer, a As Long, r2 As RECT, r3 As RECT
  Dim wipeddsd As DDSURFACEDESC2
  Dim wipedds As DirectDrawSurface7
  Dim wipedir As Integer
  
  wipeddsd.ddsCaps.lCaps = DDSCAPS_OFFSCREENPLAIN And DDSCAPS_VIDEOMEMORY
  wipeddsd.lFlags = DDSD_CAPS + DDSD_WIDTH + DDSD_HEIGHT
  wipeddsd.lWidth = 384
  wipeddsd.lHeight = 224
  Set wipedds = dD.CreateSurface(wipeddsd)
  r2.Top = 100
  r2.Bottom = r2.Top + 224
  r2.Left = CameraX
  r2.Right = r2.Left + 384
  r3.Top = 0
  r3.Bottom = 224
  r3.Left = 0
  r3.Right = 384
  wipedds.Blt r3, DDS_back, r2, DDBLT_WAIT
          
  If SoundOn Then FSOUND_PlaySound FSOUND_FREE, systemsoundHandles(1)
  
  'Randomize timer
  wipedir = Int(Rnd * 5)
  If effect > -1 Then wipedir = effect
  
  DDS_back.SetFillColor 0
  DDS_back.SetFillStyle 0
  DDS_back.SetForeColor 0
  
  If wipedir = 0 Then 'down
    For i = 0 To 112 Step 8
      a = GetTickCount
      Do: DoEvents: Loop Until GetTickCount > a
      r3.Top = 0
      r3.Bottom = r3.Top + 224 - i
      r2.Top = 100 + (i * 2)
      r2.Bottom = r2.Top + 224
      QDrawBox CameraX, 100, CameraX + 384, 100 + (i * 2), 0, 0
      QBlt r2, wipedds, r3, DDBLT_WAIT
      FlipIt
      QFlush
    Next i
  End If
  
  If wipedir = 1 Then 'up
    For i = 0 To 224 Step 8
      a = GetTickCount
      Do: DoEvents: Loop Until GetTickCount > a
      r3.Top = 0
      r3.Bottom = r3.Top + 224
      r2.Top = 100
      r2.Bottom = r2.Top + 224 - i
      QDrawBox CameraX, 100 + 224 - i, CameraX + 384, 100 + 224, 0, 0
      QBlt r2, wipedds, r3, DDBLT_WAIT
      FlipIt
      QFlush
    Next i
  End If
  
  If wipedir = 2 Then 'zoom out
    For i = 0 To 112 Step 6
      a = GetTickCount
      Do: DoEvents: Loop Until GetTickCount > a
      r2.Top = 100 + i
      r2.Bottom = r2.Top + 224 - (i * 2)
      r2.Left = CameraX + (i * 1.7)
      r2.Right = r2.Left + 384 - ((i * 1.7) * 2)
      QDrawBox CameraX, 100, CameraX + 384, 100 + 224, 0, 0
      QBlt r2, wipedds, r3, DDBLT_WAIT
      FlipIt
      QFlush
    Next i
  End If
  
  If wipedir = 3 Then 'tv line out
    For i = 0 To 110 Step 10
      a = GetTickCount
      Do: DoEvents: Loop Until GetTickCount > a
      r2.Top = 100 + i
      r2.Bottom = r2.Top + 224 - (i * 2)
      QDrawBox CameraX, 100, CameraX + 384, 100 + 224, 0, 0
      QBlt r2, wipedds, r3, DDBLT_WAIT
      FlipIt
      QFlush
    Next i
    For i = 0 To 112 Step 8
      a = GetTickCount
      Do: DoEvents: Loop Until GetTickCount > a
      r2.Left = CameraX + (i * 1.7)
      r2.Right = r2.Left + 384 - ((i * 1.7) * 2)
      QDrawBox CameraX, 100, CameraX + 384, 100 + 224, 0, 0
      QBlt r2, wipedds, r3, DDBLT_WAIT
      FlipIt
      QFlush
    Next i
  End If
  
  If wipedir = 4 Then
    For i = 0 To 384 Step 16
      a = GetTickCount
      Do: DoEvents: Loop Until GetTickCount > a
      QDrawBox CameraX, 100, CameraX + i, 324, 0, 0
      FlipIt
      QFlush
    Next i
  End If
End Sub

Public Sub NewChallengerLoop()
  Dim i As Integer, a As String, r2 As RECT, r3 As RECT, j As Integer, k As Integer
  Dim wipeddsd As DDSURFACEDESC2
  Dim wipedds As DirectDrawSurface7
  Dim wipedir As Integer
  
  wipeddsd.ddsCaps.lCaps = DDSCAPS_OFFSCREENPLAIN
  wipeddsd.lFlags = DDSD_CAPS + DDSD_WIDTH + DDSD_HEIGHT
  wipeddsd.lWidth = 384
  wipeddsd.lHeight = 224
  Set wipedds = dD.CreateSurface(wipeddsd)
  r2.Top = 100
  r2.Bottom = r2.Top + 224
  r2.Left = CameraX
  r2.Right = r2.Left + 384
  r3.Top = 0
  r3.Bottom = 224
  r3.Left = 0
  r3.Right = 384
  wipedds.Blt r3, DDS_back, r2, DDBLT_WAIT
  
  'PlayFSBSound AnnouncerFSB, ANN_22_NEWCHALLENGER
  i = 0
  a = "Here Comes A New Challenger!"
  j = 0
  
  While Not StopIt
    HandleTimingAndInput
    QBltFast CameraX, 100, wipedds, r3, DDBLTFAST_NOCOLORKEY
    'DDS_back.SetFillColor &HFFFFFF
    'DDS_back.SetForeColor &HFFFFFF
    'DDS_back.DrawBox CameraX, 212 - i, CameraX + 386, 212 + i
    QDrawBox CameraX, 212 - i, CameraX + 386, 212 + i, &HFFFFFF, &HFFFFFF
    DrawString CameraX + 342 - (Len(a) * 8) - (i * 2), 206, a, True, 1
    DrawString CameraX + 340 - (Len(a) * 8) - (i * 2), 204, Left(a, j), True, 4
    DrawString CameraX + 10, 110, CStr(k), False, 3
    If i < 16 Then
      i = i + 4
    Else
      If j < Len(a) Then
        j = j + 1
      Else
        If k < 32 Then
          k = k + 1
        Else
          Exit Sub
        End If
      End If
    End If
    FlipIt
    QFlush
  Wend
End Sub

Public Sub HandleAnimation(fighter As Integer)
  Dim advance As Boolean
  Dim retreat As Boolean
  Dim otherguy As Integer
  Dim i As Integer
  otherguy = IIf(fighter = 0, 1, 0)
  
  With players(fighter)

    If .LastKeysTimer > 0 Then
      .LastKeysTimer = .LastKeysTimer - 1
      If .LastKeysTimer = 0 Then .LastKeys = ""
    End If

    If .Locked = False Then
      'Determine if we're advancing or retreating, depending on button state and flip state...
      If Keypad(fighter).bRight Then
        If .FacingLeft = False Then
          advance = True
        Else
          If Abs(players(0).X - players(1).X) < 320 And .X > 80 Then retreat = True
        End If
      ElseIf Keypad(fighter).bLeft Then
        If .FacingLeft = True Then
          advance = True
        Else
          If Abs(players(0).X - players(1).X) < 320 And .X < 688 Then retreat = True
        End If
      End If
    End If
    
    'Clamp our position...
    'If .X < 90 Then .X = 90
    'If .X > 800 Then .X = 800
    If .X < 100 Then .X = 100
    If .X > 800 Then .X = 800
    
    If .Locked = False Then
      'Handle the cancels...
      If Keypad(fighter).bTwoPunches And .RecoveryDelay = 0 Then
        If Right(.LastKeys, 4) <> "[PP]" Then .LastKeys = .LastKeys + "[PP]"
        .LastKeysTimer = SPECIALTIME
      ElseIf Keypad(fighter).bThreePunches And .RecoveryDelay = 0 Then
        If Right(.LastKeys, 5) <> "[PPP]" Then .LastKeys = .LastKeys + "[PPP]"
        .LastKeysTimer = SPECIALTIME
      ElseIf Keypad(fighter).bTwoKicks And .RecoveryDelay = 0 Then
        If Right(.LastKeys, 4) <> "[KK]" Then .LastKeys = .LastKeys + "[KK]"
        .LastKeysTimer = SPECIALTIME
      ElseIf Keypad(fighter).bThreeKicks And .RecoveryDelay = 0 Then
        If Right(.LastKeys, 5) <> "[KKK]" Then .LastKeys = .LastKeys + "[KKK]"
        .LastKeysTimer = SPECIALTIME
      ElseIf Keypad(fighter).b1 And .RecoveryDelay = 0 Then 'And .Anims(.Anim).CancelTo(0) > -1 Then
        If .Anim <> .Anims(.Anim).CancelTo(0) Then .Frame = 0
        .Anim = .Anims(.Anim).CancelTo(0)
        If Right(.LastKeys, 4) <> "[JP]" Then .LastKeys = .LastKeys + "[JP]"
        .LastKeysTimer = SPECIALTIME
      ElseIf Keypad(fighter).b2 And .RecoveryDelay = 1 Then  'And .Anims(.Anim).CancelTo(1) > -1 Then
        If .Anim <> .Anims(.Anim).CancelTo(1) Then .Frame = 0
        .Anim = .Anims(.Anim).CancelTo(1)
        If Right(.LastKeys, 4) <> "[SP]" Then .LastKeys = .LastKeys + "[SP]"
        .LastKeysTimer = SPECIALTIME
      ElseIf Keypad(fighter).b3 And .RecoveryDelay = 0 Then  'And .Anims(.Anim).CancelTo(2) > -1 Then
        If .Anim <> .Anims(.Anim).CancelTo(2) Then .Frame = 0
        .Anim = .Anims(.Anim).CancelTo(2)
        If Right(.LastKeys, 4) <> "[FP]" Then .LastKeys = .LastKeys + "[FP]"
        .LastKeysTimer = SPECIALTIME
      ElseIf Keypad(fighter).b4 And .RecoveryDelay = 0 Then  'And .Anims(.Anim).CancelTo(3) > -1 Then
        If .Anim <> .Anims(.Anim).CancelTo(3) Then .Frame = 0
        .Anim = .Anims(.Anim).CancelTo(3)
        If Right(.LastKeys, 4) <> "[SK]" Then .LastKeys = .LastKeys + "[SK]"
        .LastKeysTimer = SPECIALTIME
      ElseIf Keypad(fighter).b5 And .RecoveryDelay = 0 Then 'And .Anims(.Anim).CancelTo(4) > -1 Then
        If .Anim <> .Anims(.Anim).CancelTo(4) Then .Frame = 0
        .Anim = .Anims(.Anim).CancelTo(4)
        If Right(.LastKeys, 4) <> "[FK]" Then .LastKeys = .LastKeys + "[FK]"
        .LastKeysTimer = SPECIALTIME
      ElseIf Keypad(fighter).b6 And .RecoveryDelay = 0 Then  'And .Anims(.Anim).CancelTo(5) > -1 Then
        If .Anim <> .Anims(.Anim).CancelTo(5) Then .Frame = 0
        .Anim = .Anims(.Anim).CancelTo(5)
        If Right(.LastKeys, 4) <> "[RK]" Then .LastKeys = .LastKeys + "[RK]"
        .LastKeysTimer = SPECIALTIME
      ElseIf advance Then 'And .Anims(.Anim).CancelTo(6) > -1 Then
        If .Anim <> .Anims(.Anim).CancelTo(6) Then .Frame = 0
        .Anim = .Anims(.Anim).CancelTo(6)
        If Right(.LastKeys, 1) <> "f" Then .LastKeys = .LastKeys + "f"
        .LastKeysTimer = SPECIALTIME
      ElseIf retreat Then 'And .Anims(.Anim).CancelTo(7) > -1 Then
        If .Anim <> .Anims(.Anim).CancelTo(7) Then .Frame = 0
        .Anim = .Anims(.Anim).CancelTo(7)
        If Right(.LastKeys, 1) <> "b" Then .LastKeys = .LastKeys + "b"
        .LastKeysTimer = SPECIALTIME
      ElseIf Keypad(fighter).bUp Then  'And .Anims(.Anim).CancelTo(8) > -1 Then
        If .Anim <> .Anims(.Anim).CancelTo(8) Then .Frame = 0
        .Anim = .Anims(.Anim).CancelTo(8)
        If Right(.LastKeys, 1) <> "u" Then .LastKeys = .LastKeys + "u"
        .LastKeysTimer = SPECIALTIME
      ElseIf Keypad(fighter).bDown Then  'And .Anims(.Anim).CancelTo(9) > -1 Then
        If .Anim <> .Anims(.Anim).CancelTo(9) Then .Frame = 0
        .Anim = .Anims(.Anim).CancelTo(9)
        If Right(.LastKeys, 1) <> "d" Then .LastKeys = .LastKeys + "d"
        .LastKeysTimer = SPECIALTIME
      Else
        If .Anim <> .Anims(.Anim).CancelTo(10) Then .Frame = 0
        .Anim = .Anims(.Anim).CancelTo(10)
      End If
      
      For i = 0 To MAXSPECIALS
        If Right(.LastKeys, Len(.Specials(i).KeySequence)) = .Specials(i).KeySequence And .Specials(i).FriendlyName <> "" Then
          'TODO: Add more checks
          LastSuper = .Specials(i).FriendlyName
          .Anim = .Specials(i).Anim
          .LastKeys = ""
          .LastKeysTimer = 0
          Exit For
        End If
      Next i
      
      '.RecoveryDelay = .Anims(.Anim).RecoveryTime
    End If
    
    'Process any Impulses for this frame...
    If .Anims(.Anim).Frames(.Frame).ImpulseX Then .VelX = .VelX + .Anims(.Anim).Frames(.Frame).ImpulseX
    If .Anims(.Anim).Frames(.Frame).ImpulseY Then .VelY = .VelY + .Anims(.Anim).Frames(.Frame).ImpulseY
    
    'Handle sounds...
    If .Anims(.Anim).Frames(.Frame).Sound > -1 Then
      'Debug.Print "Playing sound on pan " & Abs(.X - CameraX) / 2 & " <- Abs(" & CameraX & " - " & .X & ") / 2"
      PlayFSBSound FSBHandles(fighter), .Anims(.Anim).Frames(.Frame).Sound, Abs(.X - CameraX) / 2
    End If
    
    'Detect hits...
    If DetectHit(fighter, otherguy, hrDamaging, hrVulnerable) And players(otherguy).Health > 0 Then
      players(otherguy).Health = players(otherguy).Health - 5
      players(otherguy).Anim = FindAnimByName("stdHit", players(otherguy))
      players(otherguy).Frame = 0
    End If
        
    'Face the other guy when idle...
    If .Anim = 0 Then
      .VelX = 0.5 'Grind to a halt!
      If .X > players(IIf(fighter = 1, 0, 1)).X Then
        .FacingLeft = True
      ElseIf .X < players(IIf(fighter = 1, 0, 1)).X Then
        .FacingLeft = False
      End If
    End If
    
    'Handle vertical movement (gravity)...
    .Y = .Y + .VelY
    If .Y < GROUND Then
      .VelY = .VelY + 4
    Else
      If .VelY > 20 Then
        .VelY = -(.VelY / 3)
      Else
        .VelY = 0
      End If
      .Y = GROUND
    End If
    
    'Handle horizontal movement...
    .X = .X + (IIf(.FacingLeft = False, .VelX, -.VelX))
    If .VelX > 5 Then
      .VelX = .VelX - 2.5
    ElseIf .VelX > 1 Then
      .VelX = .VelX - 1
    ElseIf .VelX > 0 Then
      .VelX = .VelX - 0.5
    End If
    If .VelX < -5 Then
      .VelX = .VelX + 2.5
    ElseIf .VelX < -1 Then
      .VelX = .VelX + 1
    ElseIf .VelX < 0 Then
      .VelX = .VelX + 0.5
    End If
      
    'Increment frame counter OR loop it back if specified...
    If .Anims(.Anim).Frames(.Frame).LoopBack Then
      .Frame = .Anims(.Anim).Frames(.Frame).LoopBack
    Else
      .Frame = .Frame + 1
    End If
    
    If .RecoveryDelay > 0 Then
      .RecoveryDelay = .RecoveryDelay - 1
      If .RecoveryDelay = 0 Then .Locked = False
    End If
    
    'Handle frame counter overflows into FallTos...
    If .Frame = .Anims(.Anim).FrameC Then
      If fighter = 0 Then Debug.Print "Fallback from " & .Anims(.Anim).AnimName & ", rectime " & .Anims(.Anim).RecoveryTime
      .Frame = 0
      If .RecoveryDelay Then
        If .Anims(.Anim).FallTo(10) > -1 Then .Anim = .Anims(.Anim).FallTo(10)
      Else
        .RecoveryDelay = .Anims(.Anim).RecoveryTime
        .Locked = True
        If Keypad(fighter).b1 And .Anims(.Anim).FallTo(0) > -1 Then
          .Anim = .Anims(.Anim).FallTo(0)
        ElseIf Keypad(fighter).b2 And .Anims(.Anim).FallTo(1) > -1 Then
          .Anim = .Anims(.Anim).FallTo(1)
        ElseIf Keypad(fighter).b3 And .Anims(.Anim).FallTo(2) > -1 Then
          .Anim = .Anims(.Anim).FallTo(2)
        ElseIf advance And .Anims(.Anim).FallTo(6) > -1 Then
          .Anim = .Anims(.Anim).FallTo(6)
        ElseIf retreat And .Anims(.Anim).FallTo(7) > -1 Then
          .Anim = .Anims(.Anim).FallTo(7)
        ElseIf Keypad(fighter).bUp And .Anims(.Anim).FallTo(8) > -1 Then
          .Anim = .Anims(.Anim).FallTo(8)
        ElseIf Keypad(fighter).bDown And .Anims(.Anim).FallTo(9) > -1 Then
          .Anim = .Anims(.Anim).FallTo(9)
        ElseIf .Anims(.Anim).FallTo(10) > -1 Then
          .Anim = .Anims(.Anim).FallTo(10)
        End If
      End If
      If fighter = 0 Then Debug.Print "Falling back to "; .Anims(.Anim).AnimName
    End If
  End With
End Sub

Public Sub HandlePropAnim(a As Long, Optional winner As Long = -1)
  If props(a).Kind = pkReacter Then
    If winner = props(a).target Then props(a).Anim = 1
  End If
  If props(a).Frame = props(a).Anims(props(a).Anim).FrameC Then
    props(a).Frame = 0
    props(a).Anim = props(a).Anims(props(a).Anim).FallTo(0)
  Else
    props(a).Frame = props(a).Frame + 1
  End If
  If props(a).FaceTowards > 0 Then
    If props(a).X <= players(props(a).FaceTowards - 1).X Then
      props(a).FacingLeft = False
    Else
      props(a).FacingLeft = True
    End If
  End If
End Sub

'Given two Fighter indices and two kinds of hitrects, this will return the level of overlap if one player hits another.
'TODO: Write a player-to-prop hitscanner.
Public Function DetectHit(fighter As Integer, otherguy As Integer, kind1 As eHitrectKinds, kind2 As eHitrectKinds) As Integer
  Dim i As Integer, j As Integer, hr1 As tHitrect, hr2 As tHitrect, ft As Integer
  For i = 0 To 4
    If players(fighter).Anims(players(fighter).Anim).Frames(players(fighter).Frame).Rects(i).Kind = kind1 Then
      For j = 0 To 4
        If players(otherguy).Anims(players(otherguy).Anim).Frames(players(otherguy).Frame).Rects(j).Kind = kind2 Then
          hr1 = players(fighter).Anims(players(fighter).Anim).Frames(players(fighter).Frame).Rects(i)
          hr2 = players(otherguy).Anims(players(otherguy).Anim).Frames(players(otherguy).Frame).Rects(j)
          hr1.y1 = players(fighter).Y - players(fighter).Anims(players(fighter).Anim).Frames(players(fighter).Frame).OffY + hr1.y1
          hr1.y2 = players(fighter).Y - players(fighter).Anims(players(fighter).Anim).Frames(players(fighter).Frame).OffY + hr1.y2
          hr2.y1 = players(otherguy).Y - players(otherguy).Anims(players(otherguy).Anim).Frames(players(otherguy).Frame).OffY + hr2.y1
          hr2.y2 = players(otherguy).Y - players(otherguy).Anims(players(otherguy).Anim).Frames(players(otherguy).Frame).OffY + hr2.y2
          If players(fighter).FacingLeft = False Then
            hr1.x1 = players(fighter).X - players(fighter).Anims(players(fighter).Anim).Frames(players(fighter).Frame).OffX + hr1.x1
            hr1.x2 = players(fighter).X - players(fighter).Anims(players(fighter).Anim).Frames(players(fighter).Frame).OffX + hr1.x2
          Else
            hr1.x1 = players(fighter).X + players(fighter).Anims(players(fighter).Anim).Frames(players(fighter).Frame).OffX - hr1.x1
            hr1.x2 = players(fighter).X + players(fighter).Anims(players(fighter).Anim).Frames(players(fighter).Frame).OffX - hr1.x2
            ft = hr1.x1
            hr1.x1 = hr1.x2
            hr1.x2 = ft
          End If
          If players(otherguy).FacingLeft = False Then
            hr2.x1 = players(otherguy).X - players(otherguy).Anims(players(otherguy).Anim).Frames(players(otherguy).Frame).OffX + hr2.x1
            hr2.x2 = players(otherguy).X - players(otherguy).Anims(players(otherguy).Anim).Frames(players(otherguy).Frame).OffX + hr2.x2
          Else
            hr2.x1 = players(otherguy).X + players(otherguy).Anims(players(otherguy).Anim).Frames(players(otherguy).Frame).OffX - hr2.x1
            hr2.x2 = players(otherguy).X + players(otherguy).Anims(players(otherguy).Anim).Frames(players(otherguy).Frame).OffX - hr2.x2
            ft = hr2.x1
            hr2.x1 = hr2.x2
            hr2.x2 = ft
          End If
          If hr1.x1 < hr2.x2 And hr2.x1 < hr1.x2 And hr1.y1 < hr2.y2 And hr2.y1 < hr1.y2 Then
            DetectHit = 1 'TODO: Get overlap
            If players(fighter).FacingLeft = False Then
              DetectHit = hr1.x2 - hr2.x1
            Else
              DetectHit = hr2.x2 - hr1.x1
            End If
          Else
            DetectHit = 0
          End If
        End If
      Next j
    End If
  Next i
End Function

Public Sub LoadSurface(dest As tSurface, file As String, Optional keyed As Boolean = True, Optional Recolor As Integer = 0, Optional MeantForCardboards As Boolean = False)
  Dim ddckey As DDCOLORKEY
  Dim originalName As String
  On Error GoTo 404
  
  ddckey.high = RGB(255, 0, 255)
  ddckey.low = RGB(255, 0, 255)
  
  If MeantForCardboards = False Then
    dest.DDSD.lFlags = DDSD_CAPS
    dest.DDSD.ddsCaps.lCaps = DDSCAPS_OFFSCREENPLAIN 'And DDSCAPS_PALETTE
    
    originalName = file
    
    If Recolor > 0 Then
      Dim MyFileHead As BITMAPFILEHEADER
      Dim MyBMI As BITMAPINFOHEADER
      Dim MyColors(256) As RGBQUAD
      Dim PalPlace As Long
      Dim i As Long
      Dim path As String
      Dim newfile As String
      path = String(260, " ")
      newfile = String(260, " ")
      GetTempPath 260, path
      path = Trim(path)
      GetTempFileName path, "KFE", 0, newfile
      file = Trim(newfile)
      FileCopy originalName, file
      Open file For Binary As #1
      Get #1, , MyFileHead
      Get #1, , MyBMI
      PalPlace = Seek(1)
      If MyBMI.biBitCount > 8 Then
        Open "stderr.txt" For Append As #99
        Print #99, "Warning: Can't recolor " & originalName & " because it's not a 256-color bitmap."
        Close #99
        Kill file
        file = originalName
        GoTo Fuckit
      End If
      For i = 0 To 2 ^ MyBMI.biBitCount
        Get #1, , MyColors(i)
      Next i
      Seek #1, PalPlace
      For i = 0 To 31
        Put #1, , MyColors(i + (Recolor * 32))
      Next i
Fuckit:
      Close #1
    End If
    
    Set dest.DDS = dD.CreateSurfaceFromFile(file, dest.DDSD)
    dest.DDS.SetColorKey DDCKEY_SRCBLT, ddckey
    dest.r.Top = 0
    dest.r.Left = 0
    dest.r.Bottom = dest.DDSD.lHeight
    dest.r.Right = dest.DDSD.lWidth
    
    If Recolor > 0 Then
      Kill file
    End If
    Exit Sub
404:
    Open "stderr.txt" For Append As #99
    Print #99, "Can't load " & originalName & "." & vbCrLf & Err.Number & " - " & Err.Description
    Close #99
  
  Else
  
    dest.DDSD.lFlags = DDSD_CAPS Or DDSD_WIDTH Or DDSD_HEIGHT Or DDSD_CKSRCBLT
    dest.DDSD.ddsCaps.lCaps = DDSCAPS_TEXTURE
    dest.DDSD.ddsCaps.lCaps2 = DDSCAPS2_TEXTUREMANAGE
    dest.DDSD.ddckCKSrcBlt.high = ddckey.high
    dest.DDSD.ddckCKSrcBlt.low = ddckey.low
    Set dest.DDS = dD.CreateSurfaceFromFile(file, dest.DDSD)
  End If
End Sub

'The common string drawer. Supports two font sizes.
'TODO: Support more colors, possibly by realtime palette editing.
Public Sub DrawString(ByVal X As Integer, ByVal Y As Integer, Text As String, Optional large As Boolean = False, Optional subfont As Integer = 0, Optional spacing As Integer = 8)
  Dim c As Integer, i As Integer, r As RECT, x2 As Long, y2 As Long
  x2 = X
  y2 = Y
  If subfont = 6 Then spacing = 6
  For i = 1 To Len(Text)
    c = Asc(Mid(Text, i, 1))
    If c = 10 Then
      x2 = X
      y2 = y2 + IIf(large, 16, IIf(subfont = 6, 6, 8))
    ElseIf c = 9 Then
      'Ignore tabs
    Else
      If subfont = 6 Then 'shade the small one
        r.Top = IIf(large, 8, 0) + (subfont * 24) + 5
        r.Bottom = r.Top + IIf(large, 16, IIf(subfont = 6, 5, 8))
        r.Left = (c - Asc(" ")) * IIf(subfont = 6, 5, 8)
        r.Right = r.Left + IIf(subfont = 6, 5, 8)
        If x2 < ddsd_back.lWidth And x2 > 0 And y2 < ddsd_back.lHeight And y2 > 0 Then QBltFast x2 + 1, y2 + 1, Fonts.DDS, r, DDBLTFAST_SRCCOLORKEY
        If x2 < ddsd_back.lWidth And x2 > 0 And y2 < ddsd_back.lHeight And y2 > 0 Then QBltFast x2, y2 + 1, Fonts.DDS, r, DDBLTFAST_SRCCOLORKEY
        If x2 < ddsd_back.lWidth And x2 > 0 And y2 < ddsd_back.lHeight And y2 > 0 Then QBltFast x2 + 1, y2, Fonts.DDS, r, DDBLTFAST_SRCCOLORKEY
        If x2 < ddsd_back.lWidth And x2 > 0 And y2 < ddsd_back.lHeight And y2 > 0 Then QBltFast x2 - 1, y2 + 1, Fonts.DDS, r, DDBLTFAST_SRCCOLORKEY
        If x2 < ddsd_back.lWidth And x2 > 0 And y2 < ddsd_back.lHeight And y2 > 0 Then QBltFast x2 - 1, y2, Fonts.DDS, r, DDBLTFAST_SRCCOLORKEY
        If x2 < ddsd_back.lWidth And x2 > 0 And y2 < ddsd_back.lHeight And y2 > 0 Then QBltFast x2 - 1, y2 - 1, Fonts.DDS, r, DDBLTFAST_SRCCOLORKEY
        If x2 < ddsd_back.lWidth And x2 > 0 And y2 < ddsd_back.lHeight And y2 > 0 Then QBltFast x2 + 1, y2 - 1, Fonts.DDS, r, DDBLTFAST_SRCCOLORKEY
        If x2 < ddsd_back.lWidth And x2 > 0 And y2 < ddsd_back.lHeight And y2 > 0 Then QBltFast x2, y2 - 1, Fonts.DDS, r, DDBLTFAST_SRCCOLORKEY
      End If
      
      r.Top = IIf(large, 8, 0) + (subfont * 24)
      r.Bottom = r.Top + IIf(large, 16, IIf(subfont = 6, 5, 8))
      r.Left = (c - Asc(" ")) * IIf(subfont = 6, 5, 8)
      r.Right = r.Left + IIf(subfont = 6, 5, 8)
      If x2 < ddsd_back.lWidth And x2 > 0 And y2 < ddsd_back.lHeight And y2 > 0 Then QBltFast x2, y2, Fonts.DDS, r, DDBLTFAST_SRCCOLORKEY
      x2 = x2 + spacing
    End If
  Next i
End Sub

'Given a list of comma-delimited files, attempts to load each until it works. Falls back to "default.ogg" if
'it can't load any file, and if that fails, silence prevails.
Public Function PlaySong(filelist As String, Optional looping As Boolean) As Long
  Dim i As Integer, j As Integer
  Dim files() As String
  Dim extensions() As String
  Dim final As String
  Dim p1 As String
  Dim p2 As Long
  Dim p3 As Long
  If MusicOn = False Then Exit Function
  files = Split(Replace(filelist, ", ", ","), ",")
  extensions = Split("it, xm, s3m, mod, mid", ", ")
  If currentSong = files(0) Then Exit Function
  'Debug.Print "About to free " & currentSong & " (" & songHandle & ")"
  FMUSIC_FreeSong songHandle
  For i = 0 To UBound(files)
    For j = 0 To UBound(extensions)
      p1 = files(i) & "." & extensions(j)
      Debug.Print "Trying to load " & p1 & "..."
      GetFilePlace p1, p2, p3
      songHandle = FMUSIC_LoadSongEx(p1, p2, p3, FSOUND_NORMAL, 0, 0)
      If songHandle Then
        final = files(i) & "." & extensions(j)
        GoTo IkZieAlWatErMisIs
      End If
    Next j
  Next i
  If songHandle = 0 Then
    p1 = "utemple.it"
    GetFilePlace p1, p2, p3
    songHandle = FMUSIC_LoadSongEx(p1, p2, p3, FSOUND_NORMAL, 0, 0)
    final = "utemple.it"
  End If
IkZieAlWatErMisIs:
  If looping Then FMUSIC_SetLooping songHandle, 1
  'Debug.Print "Playing " & final & " (" & songHandle & ")"
  FMUSIC_PlaySong songHandle
  j = 64
  For i = 0 To 64
    If LCase(Left(final, InStr(final, ".") - 1)) = LCase(MusicVolume(i).filename) Then j = MusicVolume(i).volume
  Next i
  FMUSIC_SetMasterVolume songHandle, j
  currentSong = files(0)
  PlaySong = songHandle
'  Dim streamHandle As Long
'  Dim i As Integer
'  Dim files() As String
'  If MusicOn = False Then Exit Function
'  FSOUND_Stream_Close streamHandle
'  files = Split(Replace(filelist, ", ", ","), ",")
'  For i = 0 To UBound(files)
'    streamHandle = FSOUND_Stream_Open(files(i), IIf(looping, FSOUND_LOOP_NORMAL, FSOUND_NORMAL), 0, 0)
'    If streamHandle Then Exit For
'  Next i
'  If streamHandle = 0 Then streamHandle = FSOUND_Stream_Open("default.ogg", FSOUND_LOOP_NORMAL, 0, 0)
'  FSOUND_Stream_Play FSOUND_FREE, streamHandle
'  PlaySong = streamHandle
End Function

'Tries to find a Fighter's index in the Pool by it's FriendlyName. If not found, pick one at random.
'TODO: Check for bosses and yourself.
Public Function FindIndexByFriendlyName(f As String) As Integer
  Dim i As Integer
  For i = 0 To fightersinpool - 1
    If LCase(fighterpool(i).FriendlyName) = LCase(f) Then
      FindIndexByFriendlyName = i
      Exit Function
    End If
  Next i
  Randomize timer
  FindIndexByFriendlyName = Int(Rnd * fightersinpool)
End Function

'Iterates through a Fighter's animation list and returns the index for a given animation,
'or zero (stdNormal) if not found.
'TODO: Allow multiple indices with the same name, selecting one at random: multiple victories and intros and such.
Public Function FindAnimByName(f As String, fighter As tFighter) As Integer
  Dim i As Integer
  For i = 0 To MAXANIMS
    If LCase(fighter.Anims(i).AnimName) = LCase(f) Then
      FindAnimByName = i
      Exit Function
    End If
  Next i
  FindAnimByName = 0
End Function

'Given any normal string, returns just that. Given a string like "Random(foo,bar,baz,...)" returns any of the given
'values, chosen at random.
Public Function ResolveRandom(f As String) As String
  Dim a() As String, i As Integer, n As Integer
  If Left(f, 7) = "Random(" Then
    f = Mid(f, 8)
    f = Left(f, Len(f) - 1)
    a = Split(f, ",")
    ResolveRandom = a(Int(Rnd * (UBound(a) + 1)))
  Else
    ResolveRandom = f
  End If
End Function

Public Sub LoadWinsTable()
  Dim root As IXMLDOMNode
  Dim n1 As IXMLDOMNode
  Dim n2 As IXMLDOMNode
  Dim p As Integer
  Dim v As Integer
  Set X = New DOMDocument30
  X.Load "stats.xml"
  If X.parseError.errorcode <> 0 Then Exit Sub
  On Error Resume Next
  For Each root In X.firstChild.childNodes
    If root.baseName = "stats" Then
      For Each n1 In root.childNodes
        If TypeName(n1) = "IXMLDOMElement" Then
          If n1.baseName = "totals" Then
            GeneralStats.GamesStarted = Int(n1.Attributes.getNamedItem("gamesstarted").Text)
            GeneralStats.GamesWon = Int(n1.Attributes.getNamedItem("gameswon").Text)
            GeneralStats.GamesLost = Int(n1.Attributes.getNamedItem("gameslost").Text)
            GeneralStats.RoundsPlayed = Int(n1.Attributes.getNamedItem("roundsplayed").Text)
          End If
          If n1.baseName = "unlocks" Then
            GeneralStats.UnlockedMalkovic = CBool(n1.Attributes.getNamedItem("havemalkovic").Text)
          End If
        End If
      Next
    End If
    If root.baseName = "wins" Then
      For Each n1 In root.childNodes
        If TypeName(n1) = "IXMLDOMElement" Then
          Debug.Print n1.baseName
          p = FindIndexByFriendlyName(n1.baseName)
          For Each n2 In n1.childNodes
            If TypeName(n2) = "IXMLDOMElement" Then
              Debug.Print "  vs " & n2.baseName
              v = FindIndexByFriendlyName(n2.baseName)
              WinsTable(p, v, 0) = Int(n2.Attributes.getNamedItem("wins").Text)
              WinsTable(p, v, 1) = Int(n2.Attributes.getNamedItem("losses").Text)
            End If
          Next
        End If
      Next
    End If
  Next
  On Error GoTo 0
End Sub

Public Sub SaveWinsTable()
  Dim ultra As IXMLDOMElement
  Dim root As IXMLDOMElement
  Dim n1 As IXMLDOMNode
  Dim n2 As IXMLDOMElement
  Dim n3 As IXMLDOMElement
  Dim n4 As IXMLDOMAttribute
  Dim p As Integer
  Dim v As Integer
  Set X = New DOMDocument30
  
  Set ultra = X.createElement("kafestats")
  X.appendChild ultra
  
  Set root = X.createElement("stats")
  Set n2 = X.createElement("totals")
  Set n4 = X.createAttribute("gamesstarted")
  n4.value = GeneralStats.GamesStarted
  n2.Attributes.setNamedItem n4
  Set n4 = X.createAttribute("gameswon")
  n4.value = GeneralStats.GamesWon
  n2.Attributes.setNamedItem n4
  Set n4 = X.createAttribute("gameslost")
  n4.value = GeneralStats.GamesLost
  n2.Attributes.setNamedItem n4
  Set n4 = X.createAttribute("roundsplayed")
  n4.value = GeneralStats.RoundsPlayed
  n2.Attributes.setNamedItem n4
  root.appendChild n2
  Set n2 = X.createElement("unlocks")
  Set n4 = X.createAttribute("havemalkovic")
  n4.value = GeneralStats.UnlockedMalkovic
  n2.Attributes.setNamedItem n4
  root.appendChild n2
  
  ultra.appendChild root
  
  
  Set root = X.createElement("wins")
  For p = 0 To MAXFIGHTERS
    If fighterpool(p).FriendlyName <> "" Then
      Set n2 = X.createElement(fighterpool(p).FriendlyName)
      root.appendChild n2
      For v = 0 To MAXFIGHTERS
        If fighterpool(v).FriendlyName <> "" Then
          Set n3 = X.createElement(fighterpool(v).FriendlyName)
          Set n4 = X.createAttribute("wins")
          n4.value = WinsTable(p, v, 0)
          n3.Attributes.setNamedItem n4
          Set n4 = X.createAttribute("losses")
          n4.value = WinsTable(p, v, 1)
          n3.Attributes.setNamedItem n4
          n2.appendChild n3
        End If
      Next v
    End If
  Next p
  
  ultra.appendChild root
  
  X.save "stats.xml"
End Sub

Public Sub LoadXML(masterfile As String)
  Dim root As IXMLDOMNode
  Dim n1 As IXMLDOMNode
  Dim n2 As IXMLDOMNode
  Dim n3 As IXMLDOMNode
  Dim n4 As IXMLDOMNode
  Dim n5 As IXMLDOMNode
  Dim n6 As IXMLDOMNode
  Dim fighterstoload(MAXFIGHTERS) As String
  Dim ddckey As DDCOLORKEY
  Dim psofar As Integer
  Dim i As Integer
  Dim j As Integer
  Dim k As Integer
  Dim s As String
  Dim xf As String
  
  ddckey.low = RGB(255, 0, 255)
  ddckey.high = ddckey.low
  
  Set X = New DOMDocument30
  xf = LoadFile(masterfile)
  
  X.Load xf
  If X.parseError.errorcode <> 0 Then
     MsgBox Replace(Replace(Replace("Couldn't load XML file """ & masterfile & """.\n\nError: $ERROR\nLine: $CAUSE", "$ERROR", X.parseError.reason), "$CAUSE", X.parseError.srcText), "\n", vbCrLf)
     End
  End If
  KillFile xf
  
  Set root = X.firstChild
  For Each n1 In root.childNodes
    If TypeName(n1) = "IXMLDOMElement" Then
      If n1.baseName = "mcl" Then
        For Each n2 In n1.childNodes
          If n2.baseName = "character" Then
            If fightersinpool = MAXFIGHTERS Then
              MsgBox "Fighter pool is full -- not loading " & n2.Attributes.getNamedItem("href").Text & "."
            Else
              fighterstoload(fightersinpool) = n2.Attributes.getNamedItem("href").Text
              fighterpool(fightersinpool).HomeArena = n2.Attributes.getNamedItem("homearena").Text
              j = 0
              On Error Resume Next
              For Each n3 In n2.childNodes
                If TypeName(n3) = "IXMLDOMElement" Then
                  If n3.baseName = "story" Then
                    On Error Resume Next
                    k = n3.Attributes.getNamedItem("part").Text
                    Stories(j, k).Music = ResolveRandom(n3.Attributes.getNamedItem("music").Text)
                    Stories(j, k).Background = Int(ResolveRandom(n3.Attributes.getNamedItem("background").Text))
                    For Each n4 In n3.childNodes
                      If n4.baseName = "p" Then
                        Stories(j, k).Paragraphs(Stories(j, k).NumPars).Text = n4.Text
                        Stories(j, k).NumPars = Stories(j, k).NumPars + 1
                      End If
                    Next
                  End If
                  If n3.baseName = "opponent" Then
                    RosterOpponents(fightersinpool, j).FriendlyName = ResolveRandom(n3.Attributes.getNamedItem("name").Text)
                    RosterOpponents(fightersinpool, j).BackgroundOverrule = n3.Attributes.getNamedItem("arena").Text
                    RosterOpponents(fightersinpool, j).MusicOverrule = ResolveRandom(n3.Attributes.getNamedItem("song").Text)
                    RosterOpponents(fightersinpool, j).MachinemaScript = n3.Attributes.getNamedItem("script").Text
                    j = j + 1
                  End If 'opponent
                End If 'element
              Next 'character.childNodes
              On Error GoTo 0
              j = 0
              fightersinpool = fightersinpool + 1
            End If 'fightersinpool = MAXFIGHTERS
          End If 'character
        Next 'mcl.childNodes
      End If 'mcl
      If n1.baseName = "background" Then
        i = n1.Attributes.getNamedItem("id").Text
        backgrounds(i).id = i
        backgrounds(i).FriendlyName = n1.Attributes.getNamedItem("name").Text
        backgrounds(i).DDSD.lFlags = DDSD_CAPS
        backgrounds(i).DDSD.ddsCaps.lCaps = DDSCAPS_OFFSCREENPLAIN
        backgrounds(i).filename = n1.Attributes.getNamedItem("file").Text
        s = LoadFile(n1.Attributes.getNamedItem("file").Text & ".bmp")
        Set backgrounds(i).DDS = dD.CreateSurfaceFromFile(s, backgrounds(i).DDSD)
        KillFile s
        If n1.Attributes.getNamedItem("type").Text = "0" Or n1.Attributes.getNamedItem("type").Text = "still" Then
          backgrounds(i).AnimType = bgaStill
        ElseIf n1.Attributes.getNamedItem("type").Text = "1" Or n1.Attributes.getNamedItem("type").Text = "loop" Then
          backgrounds(i).AnimType = bgaLoop
          backgrounds(i).FullscreenLoopFrames = n1.Attributes.getNamedItem("frames").Text
        ElseIf n1.Attributes.getNamedItem("type").Text = "2" Or n1.Attributes.getNamedItem("type").Text = "plax" Then
          backgrounds(i).AnimType = bgaPlax
          backgrounds(i).FloorPlaneStart = n1.Attributes.getNamedItem("floorstart").Text
          backgrounds(i).DDS.SetColorKey DDCKEY_SRCBLT, ddckey
        ElseIf n1.Attributes.getNamedItem("type").Text = "3" Or n1.Attributes.getNamedItem("type").Text = "plax2" Then
          backgrounds(i).AnimType = bgaPlax2
          backgrounds(i).FloorPlaneStart = n1.Attributes.getNamedItem("floorstart").Text
          backgrounds(i).DDS.SetColorKey DDCKEY_SRCBLT, ddckey
        ElseIf n1.Attributes.getNamedItem("type").Text = "4" Or n1.Attributes.getNamedItem("type").Text = "simplescroll" Then
          backgrounds(i).AnimType = bgaSimpleScroll
          backgrounds(i).AnimSpeed = n1.Attributes.getNamedItem("speed").Text
        End If
        On Error Resume Next
        For Each n3 In n1.childNodes
          If TypeName(n3) = "IXMLDOMElement" Then
            If n3.baseName = "prop" Then
              With props(psofar)
                Debug.Print "Prop in " & backgrounds(i).FriendlyName & "..."
                Debug.Print "#" & psofar
                .BoundTo = -i
                .Kind = n3.Attributes.getNamedItem("kind").Text
                .InTheBack = n3.Attributes.getNamedItem("kind").Text
                .X = n3.Attributes.getNamedItem("x").Text
                .Y = n3.Attributes.getNamedItem("y").Text
                .FaceTowards = n3.Attributes.getNamedItem("faceto").Text
                .FacingLeft = n3.Attributes.getNamedItem("faceleft").Text
                .target = n3.Attributes.getNamedItem("target").Text
                For Each n4 In n3.childNodes
                  If TypeName(n4) = "IXMLDOMElement" Then
                    If n4.baseName = "anim" Then
                      
                      With .Anims(n4.Attributes.getNamedItem("id").Text)
                        .AnimName = n4.Attributes.getNamedItem("name").Text
                        For Each n5 In n4.childNodes
                          If n5.baseName = "frame" Then
                            .Frames(.FrameC).Left = n5.Attributes.getNamedItem("left").Text
                            .Frames(.FrameC).Top = n5.Attributes.getNamedItem("top").Text
                            .Frames(.FrameC).Width = n5.Attributes.getNamedItem("width").Text
                            .Frames(.FrameC).Height = n5.Attributes.getNamedItem("height").Text
                            .Frames(.FrameC).OffX = n5.Attributes.getNamedItem("offx").Text
                            .Frames(.FrameC).OffY = n5.Attributes.getNamedItem("offy").Text
                            On Error Resume Next
                            .Frames(.FrameC).ImpulseX = n5.Attributes.getNamedItem("impulsex").Text
                            .Frames(.FrameC).ImpulseY = n5.Attributes.getNamedItem("impulsey").Text
                            .Frames(.FrameC).LoopBack = n5.Attributes.getNamedItem("loopback").Text
                            .Frames(.FrameC).Sound = -1
                            .Frames(.FrameC).Sound = n5.Attributes.getNamedItem("sound").Text
                            On Error Resume Next
                            If n5.childNodes.length Then
                              For Each n6 In n5.childNodes
                                If n6.baseName = "rect" Then
                                  With .Frames(.FrameC).Rects(n6.Attributes.getNamedItem("id").Text)
                                    .x1 = n6.Attributes.getNamedItem("left").Text
                                    .y1 = n6.Attributes.getNamedItem("top").Text
                                    .x2 = .x1 + n6.Attributes.getNamedItem("width").Text
                                    .y2 = .y1 + n6.Attributes.getNamedItem("height").Text
                                    If n6.Attributes.getNamedItem("kind").Text = "1" Or n6.Attributes.getNamedItem("kind").Text = "hit" Then .Kind = hrVulnerable
                                    If n6.Attributes.getNamedItem("kind").Text = "2" Or n6.Attributes.getNamedItem("kind").Text = "atk" Then .Kind = hrDamaging
                                  End With 'prop frame rects
                                End If 'if rect
                              Next 'frame children
                            End If 'frame
                            .FrameC = .FrameC + 1
                          End If
                        Next 'anim children
                      End With 'with anim
                    End If 'if anim
                  End If 'xmldomelement
                Next 'prop children
                If .Kind <> pkFountain Then
                  s = LoadFile(.FilenameBase & ".bmp")
                  LoadSurface PropSurfs(psofar), s
                  .SurfaceID = psofar
                  KillFile s
                End If
              End With
              psofar = psofar + 1
            End If 'prop
          End If 'element
        Next 'background
      End If 'background
      If n1.baseName = "musicvolumes" Then
        i = 0
        For Each n2 In n1.childNodes
          If n2.baseName = "volume" Then
            MusicVolume(i).filename = n2.Attributes.getNamedItem("file").Text
            MusicVolume(i).volume = n2.Attributes.getNamedItem("volume").Text
            i = i + 1
          End If 'music
        Next 'element
      End If 'musicvolumes
    End If 'element
  Next 'root.childNodes
  
  For i = 0 To fightersinpool - 1
    xf = LoadFile(fighterstoload(i))
    X.Load xf
    If X.parseError.errorcode <> 0 Then
       MsgBox Replace(Replace(Replace("Couldn't load XML file """ & fighterstoload(i) & """.\n\nError: $ERROR\nLine: $CAUSE", "$ERROR", X.parseError.reason), "$CAUSE", X.parseError.srcText), "\n", vbCrLf)
       End
    End If
    KillFile xf
    
    Set root = X.firstChild
    With fighterpool(i)
      .FriendlyName = root.Attributes.getNamedItem("name").Text
      .FilenameBase = root.Attributes.getNamedItem("files").Text
      .FullName = .FriendlyName
      .Taunts(0) = "WARNING: No taunts for " & .FriendlyName & "!"
      .Weight = 5800
      For Each n2 In root.childNodes
        If TypeName(n2) = "IXMLDOMElement" Then
          If n2.baseName = "about" Then
            On Error Resume Next
            .FullName = n2.Attributes.getNamedItem("fullname").Text
            .HomeArea = n2.Attributes.getNamedItem("homearea").Text
            .HomeLand = n2.Attributes.getNamedItem("homeland").Text
            .Height = n2.Attributes.getNamedItem("height").Text
            .Weight = n2.Attributes.getNamedItem("weight").Text
            .FSBShortName = Int(n2.Attributes.getNamedItem("fsbname").Text)
            .FSBLongName = Int(n2.Attributes.getNamedItem("fsblongname").Text)
            On Error GoTo 0
            If n2.childNodes.length Then
              For Each n3 In n2.childNodes
                If n3.baseName = "victorytaunt" Then
                  .Taunts(n3.Attributes.getNamedItem("id").Text) = n3.Text
                End If
              Next
            End If
          End If
          If n2.baseName = "anim" Then
            With .Anims(n2.Attributes.getNamedItem("id").Text)
              .AnimName = n2.Attributes.getNamedItem("name").Text
              .RecoveryTime = 5
              If n2.Attributes.getNamedItem("id").Text = 0 Then .RecoveryTime = 1
              On Error Resume Next
              .RecoveryTime = n2.Attributes.getNamedItem("recoverytime").Text
              For Each n3 In n2.childNodes
                If n3.baseName = "fallto" Then
                  On Error Resume Next
                  For j = 0 To 10
                    .FallTo(j) = -1
                  Next j
                  For j = 0 To 5
                    .FallTo(j) = n3.Attributes.getNamedItem(Chr(Asc("a") + j)).Text
                  Next j
                  .FallTo(6) = n3.Attributes.getNamedItem("adv").Text
                  .FallTo(7) = n3.Attributes.getNamedItem("ret").Text
                  .FallTo(8) = n3.Attributes.getNamedItem("up").Text
                  .FallTo(9) = n3.Attributes.getNamedItem("down").Text
                  .FallTo(10) = n3.Attributes.getNamedItem("none").Text
                  On Error GoTo 0
                End If
                If n3.baseName = "cancelto" Then
                  On Error Resume Next
                  For j = 0 To 10
                    .CancelTo(i) = -1
                  Next j
                  For j = 0 To 5
                    .CancelTo(j) = n3.Attributes.getNamedItem(Chr(Asc("a") + j)).Text
                  Next j
                  .CancelTo(6) = n3.Attributes.getNamedItem("adv").Text
                  .CancelTo(7) = n3.Attributes.getNamedItem("ret").Text
                  .CancelTo(8) = n3.Attributes.getNamedItem("up").Text
                  .CancelTo(9) = n3.Attributes.getNamedItem("down").Text
                  .CancelTo(10) = n3.Attributes.getNamedItem("none").Text
                  On Error GoTo 0
                End If
                If n3.baseName = "frame" Then
                  .Frames(.FrameC).Left = n3.Attributes.getNamedItem("left").Text
                  .Frames(.FrameC).Top = n3.Attributes.getNamedItem("top").Text
                  .Frames(.FrameC).Width = n3.Attributes.getNamedItem("width").Text
                  .Frames(.FrameC).Height = n3.Attributes.getNamedItem("height").Text
                  .Frames(.FrameC).OffX = n3.Attributes.getNamedItem("offx").Text
                  .Frames(.FrameC).OffY = n3.Attributes.getNamedItem("offy").Text
                  On Error Resume Next
                  .Frames(.FrameC).ImpulseX = n3.Attributes.getNamedItem("impulsex").Text
                  .Frames(.FrameC).ImpulseY = n3.Attributes.getNamedItem("impulsey").Text
                  .Frames(.FrameC).LoopBack = n3.Attributes.getNamedItem("loopback").Text
                  .Frames(.FrameC).Sound = -1
                  .Frames(.FrameC).Sound = n3.Attributes.getNamedItem("sound").Text
                  On Error Resume Next
                  If n3.childNodes.length Then
                    For Each n4 In n3.childNodes
                      If n4.baseName = "rect" Then
                        With .Frames(.FrameC).Rects(n4.Attributes.getNamedItem("id").Text)
                          .x1 = n4.Attributes.getNamedItem("left").Text
                          .y1 = n4.Attributes.getNamedItem("top").Text
                          .x2 = .x1 + n4.Attributes.getNamedItem("width").Text
                          .y2 = .y1 + n4.Attributes.getNamedItem("height").Text
                          If n4.Attributes.getNamedItem("kind").Text = "1" Or n4.Attributes.getNamedItem("kind").Text = "hit" Then .Kind = hrVulnerable
                          If n4.Attributes.getNamedItem("kind").Text = "2" Or n4.Attributes.getNamedItem("kind").Text = "atk" Then .Kind = hrDamaging
                        End With
                      End If
                    Next
                  End If
                  .FrameC = .FrameC + 1
                End If
              Next
            End With
          End If
          If n2.baseName = "special" Then
            With .Specials(n2.Attributes.getNamedItem("id").Text)
              On Error Resume Next
              .Anim = n2.Attributes.getNamedItem("anim").Text
              .FriendlyName = n2.Attributes.getNamedItem("name").Text
              If n2.Attributes.getNamedItem("super").Text = "yes" Or n2.Attributes.getNamedItem("super").Text = "true" Or n2.Attributes.getNamedItem("super").Text = "1" Then .IsSuper = True
              .KeySequence = n2.Attributes.getNamedItem("keys").Text
              .LevelsNeeded = n2.Attributes.getNamedItem("levels").Text
              If n2.Attributes.getNamedItem("limit").Text = "groundonly" Or n2.Attributes.getNamedItem("limit").Text = "1" Then .Limits = slOnlyOnGround
              If n2.Attributes.getNamedItem("limit").Text = "aironly" Or n2.Attributes.getNamedItem("limit").Text = "2" Then .Limits = slOnlyInAir
              On Error GoTo 0
            End With
          End If
        End If
      Next
      .SurfaceID = i
      s = LoadFile(.FilenameBase & ".bmp")
      LoadSurface FighterSurfs(i), s, True, 0
      LoadSurface FighterSurfs(i + MAXFIGHTERS), s, True, 1
      KillFile s
'      FighterDDSD(i).lFlags = DDSD_CAPS
'      FighterDDSD(i).ddsCaps.lCaps = DDSCAPS_OFFSCREENPLAIN
'      Set FighterDDS(i) = dd.CreateSurfaceFromFile(.FilenameBase & ".bmp", FighterDDSD(i))
'      FighterDDS(i).SetColorKey DDCKEY_SRCBLT, ddckey
'
'      FighterDDSD(i + MAXFIGHTERS).lFlags = DDSD_CAPS
'      FighterDDSD(i + MAXFIGHTERS).ddsCaps.lCaps = DDSCAPS_OFFSCREENPLAIN
'      Set FighterDDS(i + MAXFIGHTERS) = dd.CreateSurfaceFromFile(.FilenameBase & ".bmp", FighterDDSD(i + MAXFIGHTERS))
'      FighterDDS(i + MAXFIGHTERS).SetColorKey DDCKEY_SRCBLT, ddckey
    End With
  Next i
End Sub

Public Sub PlayFSBSound(handle As Long, soundno As Integer, Optional pan As Integer = 128)
  Dim chan As Long
  If SoundOn = False Then Exit Sub
  If handle = 0 Then Exit Sub
  If soundno = -1 Then Exit Sub
  Debug.Print "PlayFSBSound: FSound_Stream_SetSubStream() returns " & FSOUND_Stream_SetSubStream(handle, CLng(soundno))
  chan = FSOUND_Stream_Play(FSOUND_FREE, handle)
  FSOUND_SetPan chan, pan
  FSOUND_SetVolume chan, 128
End Sub

Public Sub FlipIt()
  Dim r3 As RECT, r2 As RECT, fmodnow As Long, fmodmax As Long
  FSOUND_GetMemoryStats fmodnow, fmodmax
  
'  Dim X As Integer, Y As Integer
'  r2.Top = 24 + 32
'  r2.Left = 0
'  r2.Bottom = r2.Top + 32
'  r2.Right = r2.Left + 32
'  For X = 0 To 400 Step 32
'    For Y = 0 To 240 Step 32
'      r3.Left = CameraX + X - 8 + (framecount Mod 4)
'      r3.Top = 100 + Y
'      r3.Bottom = r3.Top + 32
'      r3.Right = r3.Left + 32
'      QCardboard r3, ParticleSurf.ddS, r2, 0.25
'    Next Y
'  Next X
  
  If ShowDebugInfo Then
    DrawString CameraX + 4, 102, "Elements: " & DDQueueSize & "/" & MaxDDQE & vbCrLf & _
                                "FMOD CPU usage: " & Format(FSOUND_GetCPUUsage, "000.00") & "%" & vbCrLf & _
                                "FMOD channels: " & FSOUND_GetChannelsPlaying & vbCrLf & _
                                "FMOD memory: " & fmodnow & "/" & fmodmax & " bytes" & vbCrLf & _
                                "FPS: " & FPS, False, 4
  End If
  
  QProcess
  r3.Top = 100
  r3.Bottom = r3.Top + 224
  r3.Left = CameraX
  r3.Right = r3.Left + 384
  dx.GetWindowRect Game.hWnd, r2
  If FullScreen = False Then r2.Top = r2.Top + 22
  
  Dim fx1 As RECT
    
  If Fun = 0 Then
    DDS_primary.Blt r2, DDS_back, r3, DDBLT_WAIT
  ElseIf Fun = 1 Then
    fx1 = r3
    fx1.Right = fx1.Left + 96
    DDS_back.Blt fx1, DDS_back, r3, DDBLT_WAIT
    r3.Right = r3.Left + 96
    DDS_primary.Blt r2, DDS_back, r3, DDBLT_WAIT
  ElseIf Fun = 2 Then
    fx1 = r3
    fx1.Right = fx1.Left + 48
    fx1.Bottom = fx1.Top + 28
    DDS_back.Blt fx1, DDS_back, r3, DDBLT_WAIT
    r3.Right = r3.Left + 48
    r3.Bottom = r3.Top + 28
    DDS_primary.Blt r2, DDS_back, r3, DDBLT_WAIT
  ElseIf Fun = 3 Then
    Dim mb As DDBLTFX
    mb.lDDFX = DDBLTFX_MIRRORLEFTRIGHT Or DDBLTFX_MIRRORUPDOWN
    DDS_primary.BltFx r2, DDS_back, r3, DDBLT_DDFX Or DDBLT_WAIT, mb
  End If
  
  DoEvents
  FPSc = FPSc + 1
End Sub

Public Sub HandleTimingAndInput()
  Dim a As Long, errpart As String, i As Integer
  a = GetTickCount
  Do
  Loop Until GetTickCount > a + speed
  framecount = framecount + 1

  didevK.GetDeviceStateKeyboard keyboard
  For a = 0 To 255
    oldkeyboard(a) = newkeyboard(a)
    If (keyboard.Key(a) And &H80) And (oldkeyboard(a) = 0 Or oldkeyboard(a) = 3) Then 'just got on
      newkeyboard(a) = 1
    ElseIf (keyboard.Key(a) And &H80) And (oldkeyboard(a) = 1 Or oldkeyboard(a) = 2) Then 'already on
      newkeyboard(a) = 2
    ElseIf Not (keyboard.Key(a) And &H80) And (oldkeyboard(a) = 1 Or oldkeyboard(a) = 2) Then 'just got off
      newkeyboard(a) = 3
    ElseIf Not (keyboard.Key(a) And &H80) And (oldkeyboard(a) = 3 Or oldkeyboard(a) = 0) Then 'already off
      newkeyboard(a) = 0
    End If
  Next a
  
  If newkeyboard(DIK_F4) And newkeyboard(56) Then Unload Game
  If newkeyboard(DIK_F12) Then TakeScreenshot
  For a = 0 To MAXPLAYERS
    If newkeyboard(Keypad(a).codeStart) = 1 Then Keypad(a).bStart = True
    If newkeyboard(Keypad(a).codeStart) = 3 Then Keypad(a).bStart = False
    If players(a).Controller <> fcCPU Then
      If newkeyboard(Keypad(a).codeLeft) = 1 Then Keypad(a).bLeft = True
      If newkeyboard(Keypad(a).codeRight) = 1 Then Keypad(a).bRight = True
      If newkeyboard(Keypad(a).codeUp) = 1 Then Keypad(a).bUp = True
      If newkeyboard(Keypad(a).codeDown) = 1 Then Keypad(a).bDown = True
      If newkeyboard(Keypad(a).codeB1) = 1 Then Keypad(a).b1 = True
      If newkeyboard(Keypad(a).codeB2) = 1 Then Keypad(a).b2 = True
      If newkeyboard(Keypad(a).codeB3) = 1 Then Keypad(a).b3 = True
      If newkeyboard(Keypad(a).codeB4) = 1 Then Keypad(a).b4 = True
      If newkeyboard(Keypad(a).codeB5) = 1 Then Keypad(a).b5 = True
      If newkeyboard(Keypad(a).codeB6) = 1 Then Keypad(a).b6 = True
    
      If newkeyboard(Keypad(a).codeLeft) = 3 Then Keypad(a).bLeft = False
      If newkeyboard(Keypad(a).codeRight) = 3 Then Keypad(a).bRight = False
      If newkeyboard(Keypad(a).codeUp) = 3 Then Keypad(a).bUp = False
      If newkeyboard(Keypad(a).codeDown) = 3 Then Keypad(a).bDown = False
      If newkeyboard(Keypad(a).codeB1) = 3 Then Keypad(a).b1 = False
      If newkeyboard(Keypad(a).codeB2) = 3 Then Keypad(a).b2 = False
      If newkeyboard(Keypad(a).codeB3) = 3 Then Keypad(a).b3 = False
      If newkeyboard(Keypad(a).codeB4) = 3 Then Keypad(a).b4 = False
      If newkeyboard(Keypad(a).codeB5) = 3 Then Keypad(a).b5 = False
      If newkeyboard(Keypad(a).codeB6) = 3 Then Keypad(a).b6 = False
    End If
  Next a
  
  If GamepadsOn Then
    Dim js As DIJOYSTATE
    For a = 0 To MAXPLAYERS
      If playerpads(a) > -1 Then
        On Error GoTo OhHolyShitOnAstick
        errpart = "poll"
        didevJ(playerpads(a)).Poll
        If errpart = "yesitfucked" Then Exit For
        errpart = "update"
        didevJ(playerpads(a)).GetDeviceStateJoystick js
        If errpart = "yesitfucked" Then Exit For
        On Error GoTo 0
        Keypad(a).bStart = IIf(js.buttons(6), True, False)
        If players(a).Controller <> fcCPU Then
          Keypad(a).b1 = IIf(js.buttons(0), True, False)
          Keypad(a).b2 = IIf(js.buttons(1), True, False)
          Keypad(a).b3 = IIf(js.buttons(2), True, False)
          Keypad(a).b4 = IIf(js.buttons(3), True, False)
          Keypad(a).b5 = IIf(js.buttons(4), True, False)
          Keypad(a).b6 = IIf(js.buttons(5), True, False)
          Keypad(a).bLeft = IIf(TranslateJoystickAxis(js.X) < 0, True, False)
          Keypad(a).bRight = IIf(TranslateJoystickAxis(js.X) > 0, True, False)
          Keypad(a).bUp = IIf(TranslateJoystickAxis(js.Y) < 0, True, False)
          Keypad(a).bDown = IIf(TranslateJoystickAxis(js.Y) > 0, True, False)
        End If
      End If
    Next a
  End If
  
  For a = 0 To MAXPLAYERS
    Keypad(a).bOnePunch = False
    Keypad(a).bTwoPunches = False
    Keypad(a).bThreePunches = False
    Keypad(a).bOneKick = False
    Keypad(a).bTwoKicks = False
    Keypad(a).bThreeKicks = False
    i = 0
    If Keypad(a).b1 Then i = i + 1
    If Keypad(a).b2 Then i = i + 1
    If Keypad(a).b3 Then i = i + 1
    If i = 1 Then
      Keypad(a).bOnePunch = True
    ElseIf i = 2 Then
      Keypad(a).bTwoPunches = True
    ElseIf i = 3 Then
      Keypad(a).bThreePunches = True
    End If
    i = 0
    If Keypad(a).b4 Then i = i + 1
    If Keypad(a).b5 Then i = i + 1
    If Keypad(a).b6 Then i = i + 1
    If i = 1 Then
      Keypad(a).bOneKick = True
    ElseIf i = 2 Then
      Keypad(a).bTwoKicks = True
    ElseIf i = 3 Then
      Keypad(a).bThreeKicks = True
    End If
  Next a
  
  Exit Sub
OhHolyShitOnAstick:
  If Err.Number = DIERR_INPUTLOST Then
    MsgBox "Lost control for player " & (a + 1) & ". Resetting to Keyboard Only."
    playerpads(a) = -1
    errpart = "yesitfucked"
    Resume Next
  Else
    MsgBox "Error while trying to " & errpart & ": " & vbCrLf & vbCrLf & Err.Number & " - " & GetFriendlyDirectXError(Err.Number)
    StopIt = True
    Resume Next
  End If
End Sub

Public Function TranslateJoystickAxis(raw As Long) As Integer
  Dim DeadZone
  DeadZone = 10000
  If raw < 32767 - DeadZone Then TranslateJoystickAxis = -1
  If raw > 32767 + DeadZone Then TranslateJoystickAxis = 1
End Function

Public Function INIRead(ByVal filename As String, ByVal INIHeader As String, ByVal Variable As String) As String
  INIRead = String(512, Chr(0))
  INIRead = Left$(INIRead, GetPrivateProfileString(INIHeader, ByVal Variable, "", INIRead, Len(INIRead), App.path & "\" & filename & ".ini"))
End Function

Public Sub INIWrite(ByVal filename As String, ByVal INIHeader As String, ByVal Variable As String, ByVal TheValue As String)
  Dim AppPath As String
  Dim TempReturn As String
  AppPath = App.path & IIf(Right(App.path, 1) = "\", "", "\")
  TempReturn = WritePrivateProfileString(INIHeader, Variable, TheValue, AppPath & filename & ".ini")
End Sub

Public Function NtS(i) As String
  NtS = Trim(Str(i))
End Function

Public Sub ComputerAI(fighter As Integer)
  Dim CPUID As Integer
  Dim OppID As Integer
  Dim myID As Integer
  CPUID = fighter
  OppID = IIf(fighter = 0, 1, 0)
  myID = IIf(fighter = 0, id1, id2)
  
  If players(CPUID).Locked = True Then Exit Sub
  
  'Clear out the keypad
  'Keypad(CPUID).bLeft = False
  'Keypad(CPUID).bRight = False

  With Brains(myID)
    
    'Release these now
    If Keypad(CPUID).b1 Then Keypad(CPUID).b1 = False
    
    If .TickTimer = 0 Then
      .TickTimer = BRAINSPEED
      'Debug.Print "Begin AI tick"
    
      Dim Distance As Integer
      Dim dD As String
      
      Distance = Abs(players(CPUID).X - players(OppID).X)
      'Debug.Print "Raw distance: " & Distance
      If Distance < 75 Then
        Distance = 0
        dD = "(face to face)"
      ElseIf Distance >= 75 And Distance < 125 Then
        Distance = 1
        dD = "(short range)"
      ElseIf Distance >= 125 And Distance < 200 Then
        Distance = 2
        dD = "(long range)"
      ElseIf Distance >= 200 Then
        Distance = 3
        dD = "(screenwide)"
      End If
      'Debug.Print "Grid distance: " & Distance & " " & dD
      
      If Keypad(CPUID).bLeft Then Keypad(CPUID).bLeft = False
      If Keypad(CPUID).bRight Then Keypad(CPUID).bRight = False
      
      Dim MyState As Integer
      Dim YourState As Integer
      Dim State As Integer
      'Debug.Print "Determining my state"
      If players(CPUID).VelY = 0 And players(CPUID).Y = GROUND Then MyState = 0
      If players(CPUID).VelY < 0 Then MyState = 1
      If players(CPUID).VelY > 0 Then MyState = 2
      
      'Debug.Print "Determining your state"
      If players(OppID).VelY = 0 And players(OppID).Y = GROUND Then YourState = 0
      If players(OppID).VelY < 0 Then YourState = 1
      If players(OppID).VelY > 0 Then YourState = 2
      
      State = (YourState * 10) + MyState
      'Debug.Print "Final state: " & State & " (0x" & Hex(State) & ")"
      
      Dim Possibility As Integer
RollPossibility:
      Possibility = Rnd * 2
      If .Matrix(State, Distance, Possibility) = 0 And Possibility > 0 Then GoTo RollPossibility
      'Debug.Print "Possibility chosen: " & Possibility
      
      Game.Caption = "Matrix(" & State & ", " & Distance & ", " & Possibility & ") = " & .Matrix(State, Distance, Possibility)
      
      'Debug.Print "Matrix says " & .Matrix(State, Distance, Possibility)
      'Debug.Print "Determining what to do"
      Select Case .Matrix(State, Distance, Possibility)
        Case 0: 'Do Nothing
        Case 1: 'Advance until next tick
          If players(CPUID).FacingLeft Then Keypad(CPUID).bLeft = True Else Keypad(CPUID).bRight = True
        Case 2: 'Retreat until next tick
          If players(CPUID).FacingLeft Then Keypad(CPUID).bRight = True Else Keypad(CPUID).bLeft = True
        Case 3: 'Crouch
          Keypad(CPUID).bDown = True
        Case 4: 'Rise from crouch
          Keypad(CPUID).bDown = False
        Case 5: 'Jump straight up
          Keypad(CPUID).bUp = True
        Case 6: 'Jump forward
          Keypad(CPUID).bUp = True
          If players(CPUID).FacingLeft Then Keypad(CPUID).bLeft = True Else Keypad(CPUID).bRight = True
        Case 7: 'Jump back
          Keypad(CPUID).bUp = True
          If players(CPUID).FacingLeft Then Keypad(CPUID).bRight = True Else Keypad(CPUID).bLeft = True
        Case 8: 'Jab
          Keypad(CPUID).b1 = True
        Case 9: 'Hard
          Keypad(CPUID).b2 = True
        Case 10: 'Fierce
          Keypad(CPUID).b3 = True
        Case 11: 'Short
          Keypad(CPUID).b4 = True
        Case 12: 'Forward
          Keypad(CPUID).b5 = True
        Case 13: 'Roundhouse
          Keypad(CPUID).b6 = True
        Case 14: 'Taunt
          players(CPUID).Anim = FindAnimByName("stdTaunt", players(CPUID))
        Case Else: 'Do Nothing anyway
      End Select
      
    Else
      .TickTimer = .TickTimer - 1
    End If
  End With
End Sub

Public Sub AddParticle(X As Long, Y As Long, Frame As Integer, DeltaH As Single, DeltaV As Single, Optional GroundLevel As Integer = -1)
  Dim i As Integer
  For i = 0 To MAXPARTICLES
    If Particles(i).Lifetime = 0 Then
      Particles(i).Lifetime = 50
      Particles(i).DeltaH = DeltaH
      Particles(i).DeltaV = DeltaV
      Particles(i).Frame = Frame
      Particles(i).X = X
      Particles(i).Y = Y
      Particles(i).GroundLevel = GroundLevel
      Exit Sub
    End If
  Next i
End Sub

Public Sub DrawParticles()
  Dim i As Integer, pr As RECT
  For i = 0 To MAXPARTICLES
    If Particles(i).Lifetime Then
      pr.Left = Particles(i).Frame * 5
      pr.Top = 0
      pr.Bottom = 5
      pr.Right = pr.Left + 5
      QBltFast Particles(i).X, Particles(i).Y, ParticleSurf.DDS, pr, DDBLTFAST_SRCCOLORKEY
      Particles(i).Lifetime = Particles(i).Lifetime - 1
      Particles(i).X = Particles(i).X + Particles(i).DeltaH
      Particles(i).Y = Particles(i).Y + Particles(i).DeltaV
      If Particles(i).Y >= Particles(i).GroundLevel Then
        Particles(i).Lifetime = 0
      End If
      Particles(i).DeltaV = Particles(i).DeltaV + 0.2
      If Particles(i).Frame = 2 Then
        Particles(i).Frame = 3
      ElseIf Particles(i).Frame = 3 Then
        Particles(i).Frame = 2
      End If
    End If
  Next i
End Sub

Public Sub CreditsLoop()
  If StopIt = True Then End
  
  Dim cx As New DOMDocument30
  Dim root As IXMLDOMElement
  Dim n1 As IXMLDOMNode
  Dim creds(255) As String
  Dim credcolors(255) As Integer
  Dim credw(255) As Integer
  Dim credc As Integer
  Dim t() As String
  Dim i As Long
  
  cx.Load "credits.xml"
  If cx.parseError.errorcode <> 0 Then
   MsgBox Replace(Replace(Replace("Couldn't load XML file ""credits.xml"".\n\nError: $ERROR\nLine: $CAUSE", "$ERROR", cx.parseError.reason), "$CAUSE", cx.parseError.srcText), "\n", vbCrLf)
   End
  End If
  
  Set root = cx.firstChild
  For Each n1 In root.childNodes
    If TypeName(n1) = "IXMLDOMElement" Then
      If n1.baseName = "header" Then
        credc = credc + 1
        credcolors(credc) = 5
        creds(credc) = n1.Text
        credw(credc) = Len(n1.Text) * 8
        credc = credc + 1
      End If
    ElseIf TypeName(n1) = "IXMLDOMText" Then
      n1.Text = Replace(n1.Text, vbTab, "")
      n1.Text = Replace(n1.Text, vbLf, "\n")
      t = Split(n1.Text, "\n")
      For i = 0 To UBound(t)
        credcolors(credc) = 4
        creds(credc) = t(i)
        credw(credc) = Len(t(i)) * 8
        credc = credc + 1
      Next i
    End If
  Next
  
  i = 0

  Background = backgrounds(2)
  'streamHandle = PlaySong("felicia.ogg", True)
  While Not StopIt
    HandleTimingAndInput
    
    DrawBackground
    
    For i = 0 To credc
      DrawString CameraX + 192 - (credw(i) / 2), 330 - framecount + (i * 16), creds(i), True, credcolors(i)
    Next i
    
    framecount = framecount + 2
    
    FlipIt
    QFlush
  Wend
FinishLoop:
  WipeOut
End Sub

Public Sub MachinemaLoop(s_script As String)
  Dim r2 As RECT, r3 As RECT, r As Integer
  Dim a As Long, b As Integer, c As Long
  Dim script(MAXSCRIPTKEYS) As String, sval As Long, sstr As String, smode As eScriptModes
  
  If StopIt = True Then End

  s_script = s_script + " "
  For a = 1 To Len(s_script)
    If Mid(s_script, a, 1) = " " Then
      Debug.Print "got eok -> script(" & b & ") = " & script(b) & ", b is now " & (b + 1)
      b = b + 1
    ElseIf Mid(s_script, a, 1) = """" Then
      Debug.Print "got strlit"
      For c = a + 1 To Len(s_script)
        If Mid(s_script, c, 1) = """" Then
          Debug.Print "got eos -> script(" & b & ") = " & script(b) & ", b is now " & (b + 1)
          Exit For
        Else
          script(b) = script(b) & Mid(s_script, c, 1)
        End If
      Next c
      a = c
    Else
      script(b) = script(b) & Mid(s_script, a, 1)
    End If
  Next a
  a = 0
  b = 0
  c = 0

  PrepareRound 0
  
  players(0).Anim = 0
  players(1).Anim = 0
  players(0).Locked = True
  players(1).Locked = True
  
  While Not StopIt
    HandleTimingAndInput
    'framecount = framecount + 1
    
    If Not CameraLocked Then CameraX = ((players(0).X + players(1).X) / 2) - (368 / 2)
    If CameraX < 80 Then CameraX = 80
    If CameraX > 434 Then CameraX = 434
          
    DrawBackground
    
    For a = 0 To MAXPROPS
      If props(a).Kind <> pkNotUsed Then
        If props(a).InTheBack = True Then
          DrawProp props(a), True
        End If
      End If
    Next a
    
    For a = 0 To 1
      r2.Left = 0
      r2.Top = 8
      r2.Right = r2.Left + 64
      r2.Bottom = r2.Top + 8
      QBltFast players(a).X - 32, GROUND - 4, ParticleSurf.DDS, r2, DDBLT_KEYSRC
    Next a
    
    If players(0).X < players(1).X Then
      DrawFighter players(1) ', True
      DrawFighter players(0) ', True
    Else
      DrawFighter players(0) ', True
      DrawFighter players(1) ', True
    End If
    
    For a = 0 To MAXPROPS
      If props(a).Kind <> pkNotUsed Then
        If props(a).InTheBack = False Then
          DrawProp props(a), True
        End If
      End If
    Next a
    
    If smode = smSay Then
      DrawString CameraX + 16 + (sval * 64), 108 + (sval * 64), sstr, True, 4
      If Keypad(0).b1 Then smode = smNormal
    ElseIf smode = smWait Then
      sval = sval - 1
      If sval = 0 Then smode = smNormal
    ElseIf smode = smNormal Then
      Debug.Print "Machinema: smode normal, script(" & b & ")" & script(b)
      Select Case script(b)
        Case "say"
          sval = Int(script(b + 1))
          sstr = script(b + 2)
          b = b + 3
          smode = smSay
        Case "anim"
          players(Int(script(b + 1))).Anim = Int(script(b + 2))
          b = b + 3
        Case "wait"
          sval = Int(script(b + 1))
          b = b + 2
          smode = smWait
        Case "sound"
          sval = Int(script(b + 1))
          Select Case sval
            Case 0: sval = FSBHandles(0)
            Case 1: sval = FSBHandles(1)
            Case 2: sval = AnnouncerFSB
          End Select
          PlayFSBSound sval, Int(script(b + 2))
          b = b + 3
        Case ""
          'TODO: End here, but bleh.
          players(0).Anim = 0
          players(1).Anim = 0
          fromMachinema = True
          Exit Sub
        Case Else
          MsgBox "Unknown command """ & script(b) & """."
      End Select
    End If
    
    Debug.Print "Machinema: frame " & framecount & ", anim " & players(0).Anim & ", frame "; players(0).Frame & "/" & players(0).Anims(players(0).Anim).FrameC
    
    HandleAnimation 0
    HandleAnimation 1
        
    DrawString CameraX + 16, 276, "sval: " & sval & vbCrLf & "sstr: " & sstr & vbCrLf & "smode: " & smode & vbCrLf & "pc: " & b, False, 3
    
    FlipIt
    QFlush
  Wend
FinishLoop:
  WipeOut
End Sub

Public Sub TakeScreenshot()
  Dim r3 As RECT, r2 As RECT, s As String
  Static sstimer As Long
  r3.Top = 100
  r3.Bottom = r3.Top + 224
  r3.Left = CameraX
  r3.Right = r3.Left + 384
  
  r2.Top = 0
  r2.Bottom = 224
  r2.Left = 0
  r2.Right = 384
  
  If GetTickCount < sstimer + 100 Then Exit Sub
  
  sstimer = GetTickCount
  
  Game.picScreenshot.Width = 384
  Game.picScreenshot.Height = 224
  DDS_back.BltToDC Game.picScreenshot.hDC, r3, r2
  
  Game.picScreenshot.Picture = Game.picScreenshot.Image
  
  s = "kafe-" & Format(Now, "ddmmyyhhmmss") & ".bmp"
  SavePicture Game.picScreenshot.Picture, s
  Debug.Print "Took screenshot -> " & s
End Sub

