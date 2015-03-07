<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="2.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:func="urn:script-functions" exclude-result-prefixes="msxsl func">

  <xsl:output method="xml" indent="yes" encoding="UTF-8"/>

  <!--ENTRY POINT-->
  <xsl:template match="/table">
    <MoneyOrders>
		<xsl:for-each select="tr[@class='']">
			<MoneyOrder>
        <Time>
					<xsl:value-of select="td[1]"/>
				</Time>
				<Currency>
					<xsl:value-of select="td[2]"/>
				</Currency>
        <!--TODO: delete ' $' in Amount--> 
        <Amount>
          <xsl:value-of  select="td[3]"/>
        </Amount>
        <Other>
          <xsl:value-of  select="td[4]/span"/>
          <xsl:text> </xsl:text>
          <xsl:value-of  select="td[5]"/>
          <xsl:text> </xsl:text>
          <xsl:value-of  select="td[6]"/>
        </Other>
      </MoneyOrder>
		</xsl:for-each>
    </MoneyOrders>
  </xsl:template>

</xsl:stylesheet>

