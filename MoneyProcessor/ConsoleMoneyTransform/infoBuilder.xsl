<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:func="urn:script-functions" exclude-result-prefixes="msxsl func">

  <xsl:output method="xml" indent="yes" />

  <!--ENTRY POINT-->
  <xsl:template match="/">
    <MoneyOrders>
		<xsl:for-each select="div">
			<xsl:element name="MoneyOrder">
	  <!--div[@class='au-deal']-->
				<Time>
					<xsl:value-of select="//small[@class='au-deal-time']"/>
				</Time>
				<Currency>
					<xsl:value-of select="//span[@class='au-deal-currency']"/>
				</Currency>
			</xsl:element>
		</xsl:for-each>
    </MoneyOrders>
  </xsl:template>

</xsl:stylesheet>

