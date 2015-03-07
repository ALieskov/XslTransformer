<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:func="urn:script-functions" 
                exclude-result-prefixes="msxsl func csharp"
                xmlns:csharp="urn:my-scripts">

  <xsl:output method="xml" indent="yes" encoding="UTF-8"/>

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
        <xsl:variable name="amount" select="td[3]"/>
        <xsl:variable name="amount1" select="csharp:Replace($amount, ' ', '')"/>
        <Amount>
          <xsl:value-of  select="csharp:Replace($amount1, '$', '')"/>
        </Amount>
        <xsl:variable name="part1" select="td[4]/span"/>
        <xsl:variable name="part2" select="td[5]"/>
        <xsl:variable name="part3" select="td[6]"/>
        <xsl:variable name="fulltext" select="concat($part1, '; ', $part2, '; ', $part3)"/>
        <Other>
          <xsl:value-of  select="csharp:SuperTrimAll($fulltext)"/>
        </Other>
      </MoneyOrder>
		</xsl:for-each>
    </MoneyOrders>
  </xsl:template>

</xsl:stylesheet>

