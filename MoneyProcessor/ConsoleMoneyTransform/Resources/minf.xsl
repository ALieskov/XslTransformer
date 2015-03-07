<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:func="urn:script-functions" 
    exclude-result-prefixes="msxsl func csharp"
    xmlns:csharp="urn:my-scripts">

  <xsl:output method="xml" indent="yes" />
  
  <msxsl:script language="C#" implements-prefix="csharp">
    <!--<msxsl:assembly name="System.String" />-->
    <msxsl:using namespace="System" />
    <msxsl:using namespace="System.Text.RegularExpressions" />
    <![CDATA[
        public string Replace(string str, string oldValue, string newValue)
        {
            return str.Replace(oldValue, newValue);
        }
        
        public string SuperTrimAll(string trim)
        {
            trim = Regex.Replace(trim, @"\r\n?|\n", " ");
            trim = Regex.Replace(trim, @"\s+", " ");
            return trim.Trim();
        }
      ]]>
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
        <xsl:variable name="amount" select="span[@class='au-deal-sum']"/>
        <xsl:variable name="amount1" select="csharp:Replace($amount, ' ', '')"/>
        <Amount>
          <xsl:value-of select="csharp:Replace($amount1, '$', '')"/>
        </Amount>
        <xsl:variable name="phone" select="span[@class='au-dealer-phone']"/>
        <xsl:variable name="comment" select="span[@class='au-deal-msg']"/>
        <xsl:variable name="fulltext" select="concat($phone, '; ', $comment)"/>
        <Other>
          <xsl:value-of select="csharp:SuperTrimAll($fulltext)"/>
        </Other>
      </MoneyOrder>
		</xsl:for-each>
    </MoneyOrders>
  </xsl:template>

</xsl:stylesheet>

