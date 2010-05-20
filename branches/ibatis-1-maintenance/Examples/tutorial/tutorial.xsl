<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	        version="1.0">
	<xsl:import href="../docbook/htmlhelp/htmlhelp.xsl"/>

	<xsl:param name="generate.legalnotice.link" select="1"/>
	<xsl:param name="suppress.navigation" select="0"/>
	<xsl:param name="admon.graphics" select="1"/>
	<xsl:param name="admon.graphics.path">gfx/</xsl:param>
	<xsl:param name="html.stylesheet" select="'default.css'"/>

	<xsl:param name="htmlhelp.hhc.binary" select="0"/>
	<xsl:param name="htmlhelp.hhc.folders.instead.books" select="0"/>
	<xsl:param name="toc.section.depth" select="4"/>

	<xsl:param name="toc.section.depth" select="4"/>
	<xsl:param name="htmlhelp.chm" select="'iBATIS.NET Tutorial 2.chm'"/>

	<xsl:template name="user.header.navigation">
	</xsl:template>

	  <xsl:template name="user.footer.navigation">
	  </xsl:template>

</xsl:stylesheet>