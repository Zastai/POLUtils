<?xml version="1.0" encoding="utf-8"?>

<xsl:transform xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
	       xmlns="http://www.w3.org/1999/xhtml"
	       xmlns:exsl="http://exslt.org/common"
	       xmlns:str="http://exslt.org/strings"
	       extension-element-prefixes="exsl str">

<xsl:output method="text" encoding="utf-8"/>

<xsl:param name="output-uri" select="'Output'"/>

<xsl:variable name="newline" select="'&#10;'"/>

<xsl:template match="/website">
  <!-- Startup Checks -->
  <xsl:if test="/website/news">
    <xsl:if test="not(/website/section[@file = /website/news/@file])">
      <xsl:message>*** WARNING: News items will link to <xsl:value-of select="/website/news/@file"/>, but there is no section with that file name.</xsl:message>
    </xsl:if>
  </xsl:if>
  <!-- Start Output -->
  <xsl:apply-templates select="section"/>
</xsl:template>

<xsl:template match="/website/section">
  <xsl:message>Creating page (<xsl:value-of select="@name"/>)...</xsl:message>
  <xsl:variable name="uri" select="concat($output-uri, '/', @file)"/>
  <exsl:document href="{$uri}" method="xml" version="1.0" encoding="iso-8859-1" omit-xml-declaration="no"
                 media-type="text/xhtml" indent="yes"
                 doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN"
                 doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html xmlns="http://www.w3.org/1999/xhtml" xsl:space="preserve">
      <xsl:message>  Page Header &amp; Navigation</xsl:message>
      <xsl:call-template name="emit-header"/>
      <body>
        <table height="100%" width="100%">
          <tr class="top-area"><td>
            <xsl:value-of select="/website/@name"/> - <xsl:value-of select="@name"/><br/>
          </td></tr>
          <tr><td>
            <xsl:message>  Sidebar</xsl:message>
            <xsl:call-template name="emit-sidebar"/>
            <xsl:message>  Content</xsl:message>
            <xsl:apply-templates/>
          </td></tr>
          <tr class="bottom-area"><td>
            Optimized to look good in <a href="http://www.mozilla.org/products/firefox/">Firefox</a>; other browsers may render this page incorrectly.<br/>
            Copyright (c) 2004,2005 Tim Van Holder.
          </td></tr>
        </table>
      </body>
    </html>
  </exsl:document>
</xsl:template>

<xsl:template match="/website/section/para">
  <p>
    <xsl:copy-of select="@*"/>
    <xsl:apply-templates mode="copy-source-tree"/>
  </p>
</xsl:template>

<xsl:template match="/website/section/news-headlines">
  <xsl:message>    News Headlines</xsl:message>
  <p>
    <h4>Recent News:</h4>
    <xsl:if test="not(/website/news/news-item)"><em>No News</em></xsl:if>
    <xsl:apply-templates select="/website/news/news-item" mode="news-headline"/>
  </p>
</xsl:template>

<xsl:template match="/website/news/news-item" mode="news-headline">
  <em><xsl:value-of select="@date"/></em>
  <xsl:text> - </xsl:text>
  <a>
    <xsl:attribute name="href"><xsl:value-of select="concat(/website/news/@file, '#', 1 + count(preceding-sibling::news-item))"/></xsl:attribute>
    <xsl:value-of select="@title"/>
  </a>
  <br/>
</xsl:template>

<xsl:template match="/website/section/news">
  <xsl:message>    News Items</xsl:message>
  <span>
    <xsl:if test="not(/website/news/news-item)"><p><em>No News</em></p></xsl:if>
    <xsl:apply-templates select="/website/news/news-item"/>
  </span>
</xsl:template>

<xsl:template match="/website/news/news-item">
  <p>
    <a>
      <xsl:attribute name="name"><xsl:value-of select="1 + count(preceding-sibling::news-item)"/></xsl:attribute>
    </a>
    <table class="news-item">
      <th>
        <xsl:value-of select="@title"/>
        <div class="news-byline">Posted on <xsl:value-of select="@date"/> by <xsl:value-of select="@author"/></div>
      </th>
      <tr class="news-body">
        <td>
          <xsl:apply-templates mode="copy-source-tree"/>
        </td>
      </tr>
    </table>
  </p>
</xsl:template>

<xsl:template match="/website/section/downloads">
  <xsl:message>    Download Links</xsl:message>
  <p>
    <xsl:if test="not(/website/downloads/group)"><em>No Downloads</em></xsl:if>
    <xsl:apply-templates select="/website/downloads/group"/>
  </p>
</xsl:template>

<xsl:template match="/website/downloads/group">
  <h4><xsl:value-of select="@name"/></h4>
  <ul>
    <xsl:if test="not(file)"><li><em>No Files</em></li></xsl:if>
    <xsl:apply-templates/>
  </ul>
</xsl:template>

<xsl:template match="/website/downloads/group/file">
  <li>
    <a>
      <xsl:attribute name="href"><xsl:value-of select="@name"/></xsl:attribute>
      <xsl:value-of select="@name"/>
    </a>
    <xsl:if test="./text()">
      <br/>
      <span class="link-description"><xsl:apply-templates mode="copy-source-tree"/></span>
    </xsl:if>
  </li>
</xsl:template>

<xsl:template match="/website/section/links">
  <xsl:message>    Web Links</xsl:message>
  <p>
    <xsl:if test="not(/website/links/group)"><em>No Links</em></xsl:if>
    <xsl:apply-templates select="/website/links/group"/>
  </p>
</xsl:template>

<xsl:template match="/website/links/group">
  <h4><xsl:value-of select="@name"/></h4>
  <ul>
    <xsl:if test="not(link)"><li><em>No Links</em></li></xsl:if>
    <xsl:apply-templates/>
  </ul>
</xsl:template>

<xsl:template match="/website/links/group/link">
  <li>
    <a>
      <xsl:attribute name="href"><xsl:value-of select="@target"/></xsl:attribute>
      <xsl:value-of select="@name"/>
    </a>
    <xsl:if test="./text()">
      <br/>
      <span class="link-description"><xsl:value-of select="."/></span>
    </xsl:if>
  </li>
</xsl:template>

<xsl:template match="*">
  <xsl:message terminate="yes">[top-level] Unhandled Element: &lt;<xsl:value-of select="name()"/>&gt;</xsl:message>
</xsl:template>

<!--++++++++++++++++++-->
<!-- Copy Source Tree -->
<!--++++++++++++++++++-->

<xsl:template mode="copy-source-tree" match="text()">
  <xsl:value-of select="."/>
</xsl:template>

<xsl:template mode="copy-source-tree" match="node()">
  <xsl:element name="{name(.)}" namespace="http://www.w3.org/1999/xhtml">
    <xsl:copy-of select="@*"/>
    <xsl:apply-templates mode="copy-source-tree"/>
  </xsl:element>
</xsl:template>

<!--+++++++++++++-->
<!-- "Functions" -->
<!--+++++++++++++-->

<xsl:template name="emit-header">
  <head>
    <title>
      <xsl:value-of select="/website/@name"/>
      <xsl:text> - </xsl:text>
      <xsl:value-of select="@name"/>
    </title>
    <meta http-equiv="Content-Type" content="text/xhtml; charset=iso-8859-1" />
    <!-- Global Navigation -->
    <link rel="stylesheet" href="site.css" media="screen"/>
    <link rel="contents">
      <xsl:attribute name="href"><xsl:value-of select="/website/section[0]/@file"/></xsl:attribute>
    </link>
    <xsl:for-each select="/website/section">
      <link rel="section">
        <xsl:attribute name="href"><xsl:value-of select="@file"/></xsl:attribute>
        <xsl:attribute name="title"><xsl:value-of select="@name"/></xsl:attribute>
      </link>
    </xsl:for-each>
    <link rel="top">
      <xsl:attribute name="href"><xsl:value-of select="/website/section[1]/@file"/></xsl:attribute>
    </link>
    <!-- Local Navigation -->
    <!-- No "up" Link -->
    <xsl:if test="count(preceding-sibling::section) != 0">
      <link rel="first">
        <xsl:attribute name="href"><xsl:value-of select="/website/section[1]/@file"/></xsl:attribute>
      </link>
      <xsl:variable name="prev-pos" select="count(preceding-sibling::section)"/>
      <link rel="prev">
        <xsl:attribute name="href"><xsl:value-of select="/website/section[$prev-pos]/@file"/></xsl:attribute>
      </link>
    </xsl:if>
    <xsl:if test="count(following-sibling::section) != 0">
      <xsl:variable name="next-pos" select="2 + count(preceding-sibling::section)"/>
      <link rel="next">
        <xsl:attribute name="href"><xsl:value-of select="/website/section[$next-pos]/@file"/></xsl:attribute>
      </link>
      <link rel="last">
        <xsl:attribute name="href"><xsl:value-of select="/website/section[last()]/@file"/></xsl:attribute>
      </link>
    </xsl:if>
  </head>
</xsl:template>

<!-- This is currently emitted on a single output line, for an as-yet undetermined reason. -->
<xsl:template name="emit-sidebar">
  <div class="sidebar">
    <table>
      <th>Pages</th>
      <tr><td>
        <xsl:for-each select="/website/section">
          <xsl:if test="preceding-sibling::section"><br/></xsl:if>
	  <a>
            <xsl:attribute name="href"><xsl:value-of select="@file"/></xsl:attribute>
            <xsl:value-of select="@name"/>
          </a>
        </xsl:for-each>
      </td></tr>
    </table>
    <br/>
  </div>
</xsl:template>

</xsl:transform>
