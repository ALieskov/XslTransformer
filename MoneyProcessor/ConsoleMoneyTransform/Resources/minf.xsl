<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:func="urn:script-functions" exclude-result-prefixes="msxsl func"
    xmlns:user="urn:my-scripts">

  <xsl:output method="xml" indent="yes" />
  <msxsl:script language="C#" implements-prefix="user">
    <!--<msxsl:assembly name="System.String" />-->
    <msxsl:using namespace="System" />
    <![CDATA[public string TrimAll(string trim){return trim.Trim();}]]>
  </msxsl:script>
  
  <!--ENTRY POINT-->
  <xsl:template match="/div">
    <MoneyOrders>
		<xsl:for-each select="div">
			<MoneyOrder>
				<Time>
					<xsl:value-of select="small[@class='au-deal-time']"/>
				</Time>
				<Currency>
					<xsl:value-of select="span[@class='au-deal-currency']"/>
				</Currency>
        <Amaunt>
          <xsl:value-of select="span[@class='au-deal-sum']"/>
        </Amaunt>
        <xsl:variable name="phone" select="span[@class='au-dealer-phone']"/>
        <xsl:variable name="comment" select="span[@class='au-deal-msg']"/>
        <Other>
          <xsl:value-of select="user:TrimAll($phone)"/>
          <xsl:text> </xsl:text>
          <xsl:value-of select="user:TrimAll($comment)"/>
        </Other>
      </MoneyOrder>
		</xsl:for-each>
    </MoneyOrders>
  </xsl:template>

</xsl:stylesheet>

