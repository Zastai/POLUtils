<?xml version="1.0" encoding="utf-8"?> <!-- $Id$ -->

<xsl:stylesheet
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
  xmlns:exsl="http://exslt.org/common" xmlns:str="http://exslt.org/strings" extension-element-prefixes="exsl str"
  >

<xsl:output method="text" encoding="utf-8"/>

<xsl:param name="output-uri" select="'Output'"/>

<xsl:template match="/website">
  <!-- Startup Checks -->
  <xsl:if test="/website/news">
    <xsl:if test="not(/website/news/@file)">
      <xsl:message terminate="yes">*** FATAL: News block has no filename specified.</xsl:message>
    </xsl:if>
    <xsl:if test="not(/website/section[@file = /website/news/@file])">
      <xsl:message>*** WARNING: RSS and news headlines will link to <xsl:value-of select="/website/news/@file"/>, but there is no section with that file name.</xsl:message>
    </xsl:if>
  </xsl:if>
  <!-- Create the RSS feed(s). -->
  <xsl:if test="/website/@news-feed">
    <xsl:if test="not(/website/@url)">
      <xsl:message terminate="yes">*** FATAL: RSS feed creation requested but no website URL set.</xsl:message>
    </xsl:if>
    <xsl:apply-templates select="/website/news" mode="rss"/>
  </xsl:if>
  <!-- Start HTML Output -->
  <xsl:apply-templates select="section"/>
</xsl:template>

<xsl:template match="/website/news" mode="rss">
  <xsl:message>Creating RSS feed (News)...</xsl:message>
  <exsl:document href="{concat($output-uri, '/', /website/@news-feed)}" method="xml" version="1.0" encoding="utf-8" omit-xml-declaration="no" media-type="application/rss+xml" indent="yes">
    <rss version="2.0">
      <channel>
	<title><xsl:value-of select="/website/@name"/></title>
	<link><xsl:value-of select="/website/@url"/></link>
	<description>Latest news items for <xsl:value-of select="/website/@name"/>.</description>
	<language>en</language>
	<xsl:for-each select="news-item">
	  <item>
	    <title><xsl:value-of select="@title"/></title>
	    <link><xsl:value-of select="concat(/website/@url, /website/news/@file, '#', position())"/></link>
	    <pubDate><xsl:value-of select="@date"/></pubDate>
	  </item>
	</xsl:for-each>
      </channel>
    </rss>
  </exsl:document>
</xsl:template>

<xsl:template match="/website/section">
  <xsl:message>Creating page (<xsl:value-of select="@name"/>)...</xsl:message>
  <exsl:document href="{concat($output-uri, '/', @file)}" method="xml" version="1.0" encoding="utf-8" omit-xml-declaration="no"
		 media-type="text/xhtml" indent="yes" xmlns="http://www.w3.org/1999/xhtml"
		 doctype-public="-//W3C//DTD XHTML 1.0 Transitional//EN"
		 doctype-system="http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
    <html>
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
	    Optimized to look good in <a href="http://www.mozilla.org/products/firefox/">Firefox</a>; other browsers may render this
	    page incorrectly.<br/>
	    Copyright © 2004,2005,2006,2007,2008 Tim Van Holder.
	  </td></tr>
	</table>
      </body>
    </html>
  </exsl:document>
</xsl:template>

<xsl:template match="/website/section/para" xmlns="http://www.w3.org/1999/xhtml">
  <p>
    <xsl:copy-of select="@*"/>
    <xsl:apply-templates mode="copy-source-tree"/>
  </p>
</xsl:template>

<xsl:template match="/website/section/news-headlines" xmlns="http://www.w3.org/1999/xhtml">
  <xsl:message>    News Headlines</xsl:message>
  <p>
    <h4>Recent News:</h4>
    <xsl:if test="not(/website/news/news-item)"><em>No News</em></xsl:if>
    <xsl:apply-templates select="/website/news/news-item[position() &lt; 6]" mode="news-headline"/>
  </p>
</xsl:template>

<xsl:template match="/website/news/news-item" mode="news-headline" xmlns="http://www.w3.org/1999/xhtml">
  <em><xsl:value-of select="@date"/></em>
  <xsl:text> - </xsl:text>
  <a>
    <xsl:if test="/website/news/@file">
      <xsl:attribute name="href"><xsl:value-of select="concat(/website/news/@file, '#', position())"/></xsl:attribute>
    </xsl:if>
    <xsl:value-of select="@title"/>
  </a>
  <br/>
</xsl:template>

<xsl:template match="/website/section/news" xmlns="http://www.w3.org/1999/xhtml">
  <xsl:message>    News Items</xsl:message>
  <span>
    <xsl:if test="not(/website/news/news-item)"><p><em>No News</em></p></xsl:if>
    <xsl:apply-templates select="/website/news/news-item"/>
  </span>
</xsl:template>

<xsl:template match="/website/news/news-item" xmlns="http://www.w3.org/1999/xhtml">
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

<xsl:template match="/website/section/downloads" xmlns="http://www.w3.org/1999/xhtml">
  <xsl:message>    Download Links</xsl:message>
  <p>
    <xsl:if test="not(/website/downloads/group)"><em>No Downloads</em></xsl:if>
    <xsl:apply-templates select="/website/downloads/group"/>
  </p>
</xsl:template>

<xsl:template match="/website/downloads/group" xmlns="http://www.w3.org/1999/xhtml">
  <h4><xsl:value-of select="@name"/></h4>
  <xsl:apply-templates select="description"/>
  <ul>
    <xsl:if test="not(file)"><li><em>No Files</em></li></xsl:if>
    <xsl:apply-templates select="file"/>
  </ul>
</xsl:template>

<xsl:template match="/website/downloads/group/description" xmlns="http://www.w3.org/1999/xhtml">
   <xsl:apply-templates mode="copy-source-tree"/>
</xsl:template>
  
<xsl:template match="/website/downloads/group/file" xmlns="http://www.w3.org/1999/xhtml">
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

<xsl:template match="/website/section/links" xmlns="http://www.w3.org/1999/xhtml">
  <xsl:message>    Web Links</xsl:message>
  <p>
    <xsl:if test="not(/website/links/group)"><em>No Links</em></xsl:if>
    <xsl:apply-templates select="/website/links/group"/>
  </p>
</xsl:template>

<xsl:template match="/website/links/group" xmlns="http://www.w3.org/1999/xhtml">
  <h4><xsl:value-of select="@name"/></h4>
  <ul>
    <xsl:if test="not(link)"><li><em>No Links</em></li></xsl:if>
    <xsl:apply-templates select="link"/>
  </ul>
</xsl:template>

<xsl:template match="/website/links/group/link" xmlns="http://www.w3.org/1999/xhtml">
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
  <xsl:element name="{name(.)}">
    <xsl:copy-of select="@*"/>
    <xsl:apply-templates mode="copy-source-tree"/>
  </xsl:element>
</xsl:template>

<!--+++++++++++++-->
<!-- "Functions" -->
<!--+++++++++++++-->

<xsl:template name="emit-header" xmlns="http://www.w3.org/1999/xhtml">
  <head>
    <title>
      <xsl:value-of select="/website/@name"/>
      <xsl:text> - </xsl:text>
      <xsl:value-of select="@name"/>
    </title>
    <meta http-equiv="Content-Type" content="text/xhtml; charset=utf-8" />
    <link rel="stylesheet" href="site.css" media="screen"/>
    <xsl:if test="/website/@news-feed and (.//news or .//news-headlines)">
      <link rel="alternate" type="application/rss+xml" title="RSS" href="{concat(/website/@url, /website/@news-feed)}" />
    </xsl:if>
    <!-- Global Navigation -->
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
<xsl:template name="emit-sidebar" xmlns="http://www.w3.org/1999/xhtml">
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
	  <xsl:if test="/website/@news-feed and .//news">
	    <xsl:text> </xsl:text>
	    <a href="{concat(/website/@url, /website/@news-feed)}"><img border="0" alt="RSS Feed" src="rss.png"/></a>
	  </xsl:if>
	</xsl:for-each>
      </td></tr>
    </table>
    <br/>
  </div>
</xsl:template>

</xsl:stylesheet>
