<?xml version="1.0" encoding="utf-8" ?>
<!-- $Id$ -->
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">

  <xsl:output method="xml" version="1.0" encoding="utf-8" omit-xml-declaration="no" media-type="text/xml" indent="yes" />

  <!-- Step 1: Try to figure out which format it is. -->
  <xsl:template match="/">
    <xsl:choose>
      <xsl:when test="/ffxi-item-info">
	<xsl:apply-templates mode="format-1" select="/ffxi-item-info"/>
      </xsl:when>
      <xsl:when test="/ItemList">
	<xsl:apply-templates mode="format-2" select="/ItemList"/>
      </xsl:when>
      <xsl:otherwise>
	<xsl:message terminate="yes">Sorry, this file could not be recognized as an item dump.</xsl:message>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="error-exit">
    <xsl:message terminate="yes">An error occurred trying to process this file.</xsl:message>
  </xsl:template>
  
  <!--==========-->
  <!-- Format 1 -->
  <!--==========-->

  <xsl:template mode="format-1" match="/ffxi-item-info">
    <ItemList>
      <xsl:apply-templates mode="format-1" select="data-language" />
      <xsl:apply-templates mode="format-1" select="data-type" />
      <xsl:for-each select="item">
	<Item>
	  <xsl:for-each select="field">
	    <xsl:apply-templates mode="format-1" select="." />
	  </xsl:for-each>
	  <xsl:apply-templates mode="format-1" select="icon" />
	  <!-- Calculate DPS -->
	  <xsl:if test="./field[@name = 'Type'] = 'Weapon'">
	    <DPS><xsl:value-of select="round(6000 * number(./field[@name = 'Damage']) div number(./field[@name = 'Delay'])) div 100"/></DPS>
	  </xsl:if>
	</Item>
      </xsl:for-each>
    </ItemList>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/data-language">
    <xsl:attribute name="Language">
      <xsl:value-of select="."/>
    </xsl:attribute>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/data-type">
    <xsl:attribute name="Type">
      <xsl:value-of select="."/>
    </xsl:attribute>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field">
    <!-- Default field rule: map the field name and preserve the value -->
    <xsl:variable name="new-field-name">
      <xsl:choose>
	<xsl:when test="@name = 'ResourceID'">MysteryField</xsl:when>
	<xsl:otherwise><xsl:value-of select="@name"/></xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:element name="{$new-field-name}">
      <xsl:value-of select="."/>
    </xsl:element>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Flags']">
    <xsl:variable name="flag01">0<xsl:if test="contains(., 'Flag00')"><xsl:value-of select="1"/></xsl:if></xsl:variable>
    <xsl:variable name="flag02">0<xsl:if test="contains(., 'Flag01')"><xsl:value-of select="2"/></xsl:if></xsl:variable>
    <xsl:variable name="flag03">0<xsl:if test="contains(., 'Flag02')"><xsl:value-of select="4"/></xsl:if></xsl:variable>
    <xsl:variable name="flag04">0<xsl:if test="contains(., 'Flag03')"><xsl:value-of select="8"/></xsl:if></xsl:variable>
    <xsl:variable name="flag05">0<xsl:if test="contains(., 'Flag04')"><xsl:value-of select="16"/></xsl:if></xsl:variable>
    <xsl:variable name="flag06">0<xsl:if test="contains(., 'Flag05') or contains(., 'Inscribable')"><xsl:value-of select="32"/></xsl:if></xsl:variable>
    <xsl:variable name="flag07">0<xsl:if test="contains(., 'Flag06') or contains(., 'NoAuction') or contains(., 'Ex')"><xsl:value-of select="64"/></xsl:if></xsl:variable>
    <xsl:variable name="flag08">0<xsl:if test="contains(., 'Flag07') or contains(., 'Scroll')"><xsl:value-of select="128"/></xsl:if></xsl:variable>
    <xsl:variable name="flag09">0<xsl:if test="contains(., 'Flag08') or contains(., 'Linkshell')"><xsl:value-of select="256"/></xsl:if></xsl:variable>
    <xsl:variable name="flag10">0<xsl:if test="contains(., 'Flag09') or contains(., 'CanUse')"><xsl:value-of select="512"/></xsl:if></xsl:variable>
    <xsl:variable name="flag11">0<xsl:if test="contains(., 'Flag10') or contains(., 'CanTradeNPC')"><xsl:value-of select="1024"/></xsl:if></xsl:variable>
    <xsl:variable name="flag12">0<xsl:if test="contains(., 'Flag11') or contains(., 'CanEquip')"><xsl:value-of select="2048"/></xsl:if></xsl:variable>
    <xsl:variable name="flag13">0<xsl:if test="contains(., 'Flag12') or contains(., 'NoSale')"><xsl:value-of select="4096"/></xsl:if></xsl:variable>
    <xsl:variable name="flag14">0<xsl:if test="contains(., 'Flag13') or contains(., 'NoDelivery') or contains(., 'Ex')"><xsl:value-of select="08192"/></xsl:if></xsl:variable>
    <xsl:variable name="flag15">0<xsl:if test="contains(., 'Flag14') or contains(., 'NoTradePC') or contains(., 'Ex')"><xsl:value-of select="16384"/></xsl:if></xsl:variable>
    <xsl:variable name="flag16">0<xsl:if test="contains(., 'Flag15') or contains(., 'Rare')"><xsl:value-of select="32768"/></xsl:if></xsl:variable>
    <xsl:element name="{@name}">
      <xsl:call-template name="format-number-as-hex">
	<xsl:with-param name="number" select="$flag01 + $flag02 + $flag03 + $flag04 + $flag05 + $flag06 + $flag07 + $flag08 + $flag09 + $flag10 + $flag11 + $flag12 + $flag13 + $flag14 + $flag15 + $flag16"/>
      </xsl:call-template>
    </xsl:element>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Jobs']">
    <xsl:variable name="flag01">0</xsl:variable> <!-- Unused -->
    <xsl:variable name="flag02">0<xsl:if test=". = 'All' or contains(., 'WAR')"><xsl:value-of select="2"/></xsl:if></xsl:variable>
    <xsl:variable name="flag03">0<xsl:if test=". = 'All' or contains(., 'MNK')"><xsl:value-of select="4"/></xsl:if></xsl:variable>
    <xsl:variable name="flag04">0<xsl:if test=". = 'All' or contains(., 'WHM')"><xsl:value-of select="8"/></xsl:if></xsl:variable>
    <xsl:variable name="flag05">0<xsl:if test=". = 'All' or contains(., 'BLM')"><xsl:value-of select="16"/></xsl:if></xsl:variable>
    <xsl:variable name="flag06">0<xsl:if test=". = 'All' or contains(., 'RDM')"><xsl:value-of select="32"/></xsl:if></xsl:variable>
    <xsl:variable name="flag07">0<xsl:if test=". = 'All' or contains(., 'THF')"><xsl:value-of select="64"/></xsl:if></xsl:variable>
    <xsl:variable name="flag08">0<xsl:if test=". = 'All' or contains(., 'PLD')"><xsl:value-of select="128"/></xsl:if></xsl:variable>
    <xsl:variable name="flag09">0<xsl:if test=". = 'All' or contains(., 'DRK')"><xsl:value-of select="256"/></xsl:if></xsl:variable>
    <xsl:variable name="flag10">0<xsl:if test=". = 'All' or contains(., 'BST')"><xsl:value-of select="512"/></xsl:if></xsl:variable>
    <xsl:variable name="flag11">0<xsl:if test=". = 'All' or contains(., 'BRD')"><xsl:value-of select="1024"/></xsl:if></xsl:variable>
    <xsl:variable name="flag12">0<xsl:if test=". = 'All' or contains(., 'RNG')"><xsl:value-of select="2048"/></xsl:if></xsl:variable>
    <xsl:variable name="flag13">0<xsl:if test=". = 'All' or contains(., 'SAM')"><xsl:value-of select="4096"/></xsl:if></xsl:variable>
    <xsl:variable name="flag14">0<xsl:if test=". = 'All' or contains(., 'NIN')"><xsl:value-of select="08192"/></xsl:if></xsl:variable>
    <xsl:variable name="flag15">0<xsl:if test=". = 'All' or contains(., 'DRG')"><xsl:value-of select="16384"/></xsl:if></xsl:variable>
    <xsl:variable name="flag16">0<xsl:if test=". = 'All' or contains(., 'SMN')"><xsl:value-of select="32768"/></xsl:if></xsl:variable>
    <xsl:element name="{@name}">
      <xsl:call-template name="format-number-as-hex">
	<xsl:with-param name="number" select="$flag01 + $flag02 + $flag03 + $flag04 + $flag05 + $flag06 + $flag07 + $flag08 + $flag09 + $flag10 + $flag11 + $flag12 + $flag13 + $flag14 + $flag15 + $flag16"/>
      </xsl:call-template>
    </xsl:element>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Slots']">
    <xsl:variable name="flag01">0<xsl:if test=". = 'All' or contains(., 'Main')"><xsl:value-of select="1"/></xsl:if></xsl:variable>
    <xsl:variable name="flag02">0<xsl:if test=". = 'All' or contains(., 'Sub')"><xsl:value-of select="2"/></xsl:if></xsl:variable>
    <xsl:variable name="flag03">0<xsl:if test=". = 'All' or contains(., 'Range')"><xsl:value-of select="4"/></xsl:if></xsl:variable>
    <xsl:variable name="flag04">0<xsl:if test=". = 'All' or contains(., 'Ammo')"><xsl:value-of select="8"/></xsl:if></xsl:variable>
    <xsl:variable name="flag05">0<xsl:if test=". = 'All' or contains(., 'Head')"><xsl:value-of select="16"/></xsl:if></xsl:variable>
    <xsl:variable name="flag06">0<xsl:if test=". = 'All' or contains(., 'Body')"><xsl:value-of select="32"/></xsl:if></xsl:variable>
    <xsl:variable name="flag07">0<xsl:if test=". = 'All' or contains(., 'Hands')"><xsl:value-of select="64"/></xsl:if></xsl:variable>
    <xsl:variable name="flag08">0<xsl:if test=". = 'All' or contains(., 'Legs')"><xsl:value-of select="128"/></xsl:if></xsl:variable>
    <xsl:variable name="flag09">0<xsl:if test=". = 'All' or contains(., 'Feet')"><xsl:value-of select="256"/></xsl:if></xsl:variable>
    <xsl:variable name="flag10">0<xsl:if test=". = 'All' or contains(., 'Neck')"><xsl:value-of select="512"/></xsl:if></xsl:variable>
    <xsl:variable name="flag11">0<xsl:if test=". = 'All' or contains(., 'Waist')"><xsl:value-of select="1024"/></xsl:if></xsl:variable>
    <xsl:variable name="flag12">0<xsl:if test=". = 'All' or contains(., 'LEar') or contains(., 'Ears')"><xsl:value-of select="2048"/></xsl:if></xsl:variable>
    <xsl:variable name="flag13">0<xsl:if test=". = 'All' or contains(., 'REar') or contains(., 'Ears')"><xsl:value-of select="4096"/></xsl:if></xsl:variable>
    <xsl:variable name="flag14">0<xsl:if test=". = 'All' or contains(., 'LRing') or contains(., 'Rings')"><xsl:value-of select="08192"/></xsl:if></xsl:variable>
    <xsl:variable name="flag15">0<xsl:if test=". = 'All' or contains(., 'RRing') or contains(., 'Rings')"><xsl:value-of select="16384"/></xsl:if></xsl:variable>
    <xsl:variable name="flag16">0<xsl:if test=". = 'All' or contains(., 'Back') or contains(., 'Back')"><xsl:value-of select="32768"/></xsl:if></xsl:variable>
    <xsl:element name="{@name}">
      <xsl:call-template name="format-number-as-hex">
	<xsl:with-param name="number" select="$flag01 + $flag02 + $flag03 + $flag04 + $flag05 + $flag06 + $flag07 + $flag08 + $flag09 + $flag10 + $flag11 + $flag12 + $flag13 + $flag14 + $flag15 + $flag16"/>
      </xsl:call-template>
    </xsl:element>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Races']">
    <xsl:variable name="flag01">0</xsl:variable>
    <xsl:variable name="flag02">0<xsl:if test=". = 'All' or contains(., 'HumeMale') or (contains(., 'Hume') and not (contains(., 'HumeFemale'))) or (contains(., 'Male') and not (contains(., 'ElvaanMale') or contains(., 'TarutaruMale')))"><xsl:value-of select="2"/></xsl:if></xsl:variable>
    <xsl:variable name="flag03">0<xsl:if test=". = 'All' or contains(., 'HumeFemale') or (contains(., 'Hume') and not (contains(., 'HumeMale'))) or (contains(., 'Female') and not (contains(., 'ElvaanFemale') or contains(., 'TarutaruFemale')))"><xsl:value-of select="4"/></xsl:if></xsl:variable>
    <xsl:variable name="flag04">0<xsl:if test=". = 'All' or contains(., 'ElvaanMale') or (contains(., 'Elvaan') and not (contains(., 'ElvaanFemale'))) or (contains(., 'Male') and not (contains(., 'HumeMale') or contains(., 'TarutaruMale')))"><xsl:value-of select="8"/></xsl:if></xsl:variable>
    <xsl:variable name="flag05">0<xsl:if test=". = 'All' or contains(., 'ElvaanFemale') or (contains(., 'Elvaan') and not (contains(., 'ElvaanMale'))) or (contains(., 'Female') and not (contains(., 'HumeFemale') or contains(., 'TarutaruFemale')))"><xsl:value-of select="16"/></xsl:if></xsl:variable>
    <xsl:variable name="flag06">0<xsl:if test=". = 'All' or contains(., 'TarutaruMale') or (contains(., 'Tarutaru') and not (contains(., 'TarutaruFemale'))) or (contains(., 'Male') and not (contains(., 'HumeMale') or contains(., 'ElvaanMale')))"><xsl:value-of select="32"/></xsl:if></xsl:variable>
    <xsl:variable name="flag07">0<xsl:if test=". = 'All' or contains(., 'TarutaruFemale') or (contains(., 'Tarutaru') and not (contains(., 'TarutaruMale'))) or (contains(., 'Female') and not (contains(., 'HumeFemale') or contains(., 'ElvaanFemale')))"><xsl:value-of select="64"/></xsl:if></xsl:variable>
    <xsl:variable name="flag08">0<xsl:if test=". = 'All' or contains(., 'Mithra') or (contains(., 'Female') and not (contains(., 'HumeFemale') or contains(., 'ElvaanFemale') or contains(., 'TarutaruFemale')))"><xsl:value-of select="128"/></xsl:if></xsl:variable>
    <xsl:variable name="flag09">0<xsl:if test=". = 'All' or contains(., 'Galka') or (contains(., 'Male') and not (contains(., 'HumeMale') or contains(., 'ElvaanMale') or contains(., 'TarutaruMale')))"><xsl:value-of select="256"/></xsl:if></xsl:variable>
    <xsl:variable name="flag10">0</xsl:variable>
    <xsl:variable name="flag11">0</xsl:variable>
    <xsl:variable name="flag12">0</xsl:variable>
    <xsl:variable name="flag13">0</xsl:variable>
    <xsl:variable name="flag14">0</xsl:variable>
    <xsl:variable name="flag15">0</xsl:variable>
    <xsl:variable name="flag16">0</xsl:variable>
    <xsl:element name="{@name}">
      <xsl:call-template name="format-number-as-hex">
	<xsl:with-param name="number" select="$flag01 + $flag02 + $flag03 + $flag04 + $flag05 + $flag06 + $flag07 + $flag08 + $flag09 + $flag10 + $flag11 + $flag12 + $flag13 + $flag14 + $flag15 + $flag16"/>
      </xsl:call-template>
    </xsl:element>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Skill']">
    <xsl:element name="{@name}">
      <xsl:choose>
	<xsl:when test=". = 'None'">00</xsl:when>
	<xsl:when test=". = 'HandToHand'">01</xsl:when>
	<xsl:when test=". = 'Dagger'">02</xsl:when>
	<xsl:when test=". = 'Sword'">03</xsl:when>
	<xsl:when test=". = 'GreatSword'">04</xsl:when>
	<xsl:when test=". = 'Axe'">05</xsl:when>
	<xsl:when test=". = 'GreatAxe'">06</xsl:when>
	<xsl:when test=". = 'Scythe'">07</xsl:when>
	<xsl:when test=". = 'PoleArm'">08</xsl:when>
	<xsl:when test=". = 'Katana'">09</xsl:when>
	<xsl:when test=". = 'GreatKatana'">0A</xsl:when>
	<xsl:when test=". = 'Club'">0B</xsl:when>
	<xsl:when test=". = 'Staff'">0C</xsl:when>
	<xsl:when test=". = 'Ranged'">19</xsl:when>
	<xsl:when test=". = 'Marksmanship'">1A</xsl:when>
	<xsl:when test=". = 'Thrown'">1B</xsl:when>
	<xsl:when test=". = 'DivineMagic'">20</xsl:when>
	<xsl:when test=". = 'HealingMagic'">21</xsl:when>
	<xsl:when test=". = 'EnhancingMagic'">22</xsl:when>
	<xsl:when test=". = 'EnfeeblingMagic'">23</xsl:when>
	<xsl:when test=". = 'ElementalMagic'">24</xsl:when>
	<xsl:when test=". = 'DarkMagic'">25</xsl:when>
	<xsl:when test=". = 'SummoningMagic'">26</xsl:when>
	<xsl:when test=". = 'Ninjutsu'">27</xsl:when>
	<xsl:when test=". = 'Singing'">28</xsl:when>
	<xsl:when test=". = 'StringInstrument'">29</xsl:when>
	<xsl:when test=". = 'WindInstrument'">2A</xsl:when>
	<xsl:when test=". = 'Fishing'">30</xsl:when>
	<xsl:otherwise>FF</xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/field[@name = 'Type']">
    <xsl:element name="{@name}">
      <xsl:choose>
	<xsl:when test=". = 'Nothing'">0000</xsl:when>
	<xsl:when test=". = 'Item'">0001</xsl:when>
	<xsl:when test=". = 'QuestItem'">0002</xsl:when>
	<xsl:when test=". = 'Fish'">0003</xsl:when>
	<xsl:when test=". = 'Weapon'">0004</xsl:when>
	<xsl:when test=". = 'Armor'">0005</xsl:when>
	<xsl:when test=". = 'Linkshell'">0006</xsl:when>
	<xsl:when test=". = 'UsableItem'">0007</xsl:when>
	<xsl:when test=". = 'Crystal'">0008</xsl:when>
	<xsl:when test=". = 'Unknown'">0009</xsl:when>
	<xsl:when test=". = 'Furnishing'">000A</xsl:when>
	<xsl:when test=". = 'Plant'">000B</xsl:when>
	<xsl:when test=". = 'Flowerpot'">000C</xsl:when>
	<xsl:when test=". = 'Material'">000D</xsl:when>
	<xsl:when test=". = 'Mannequin'">000E</xsl:when>
	<xsl:when test=". = 'Book'">000F</xsl:when>
	<xsl:otherwise>FFFF</xsl:otherwise>
      </xsl:choose>
    </xsl:element>
  </xsl:template>

  <xsl:template mode="format-1" match="/ffxi-item-info/item/icon">
    <Icon>
      <xsl:if test="@category">
	<xsl:attribute name="Category">
	  <xsl:value-of select="@category"/>
	</xsl:attribute>
      </xsl:if>
      <xsl:if test="@id">
	<xsl:attribute name="ID">
	  <xsl:value-of select="@id"/>
	</xsl:attribute>
      </xsl:if>
      <xsl:if test="@format">
	<xsl:attribute name="Format">
	  <xsl:value-of select="@format"/>
	</xsl:attribute>
      </xsl:if>
      <xsl:value-of select="." />
    </Icon>
  </xsl:template>

  <xsl:template mode="format-1" match="*">
    <xsl:message>Unhandled element: <xsl:value-of select="name(.)"/></xsl:message>
    <xsl:call-template name="error-exit" />
  </xsl:template>

  <xsl:template name="format-number-as-hex">
    <xsl:param name="number" select="0" />

    <xsl:variable name="this-nybble" select="$number mod 16" />
    <xsl:variable name="rest" select="floor($number div 16)" />

    <xsl:if test="$rest != 0">
      <xsl:call-template name="format-number-as-hex">
	<xsl:with-param name="number" select="$rest"/>
      </xsl:call-template>
    </xsl:if>

    <xsl:choose>
      <xsl:when test="$this-nybble &gt;= 10">
	<xsl:value-of select="translate(string($this-nybble - 10), '012345', 'ABCDEF')"/>
      </xsl:when>
      <xsl:otherwise>
	<xsl:value-of select="$this-nybble"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>
  
  <!--==========-->
  <!-- Format 2 -->
  <!--==========-->

  <xsl:template mode="format-2" match="/ItemList">
    <!-- This is the current format, so just copy verbatim -->
    <xsl:copy-of select="." />
  </xsl:template>

</xsl:stylesheet>
