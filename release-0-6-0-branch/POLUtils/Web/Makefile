website: Output/index.xhtml Output/site.css

Output/site.css: pebbles.css
	@cp -v '$<' '$@'

Output/index.xhtml: WebSite.xslt WebSite.xml
	@xsltproc WebSite.xslt WebSite.xml
