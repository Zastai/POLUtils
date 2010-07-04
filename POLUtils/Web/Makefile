# $Id$

website: Output/index.html Output/site.css Output/rss.png

Output/site.css: pebbles.css
	@cp -v '$<' '$@'

Output/index.html: WebSite.xslt WebSite.xml
	@xsltproc WebSite.xslt WebSite.xml

Output/%: %
	@cp -v '$<' '$@'
