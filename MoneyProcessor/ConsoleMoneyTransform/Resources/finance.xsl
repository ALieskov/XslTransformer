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
  
  <xsl:param name="neededOperation"  select="'buy'"/>
  <xsl:param name="intrestedCurrencyName" select="'USD'"/>

  <!--ENTRY POINT-->
  <xsl:template match="/tbody">
    <MoneyOrders>
		<xsl:for-each select="tr">
      <xsl:variable name="currencyName" select="td[2]"/>
      <xsl:variable name="operation" select="td[3]"/>
      
      <!--<xsl:if test="(($currencyName = 'USD') and ($operation = 'Куплю'))">-->
      <xsl:if test="($currencyName = $intrestedCurrencyName) and ($operation = $neededOperation)">
			<MoneyOrder>
				<Time>
					<xsl:value-of select="td[1]"/>
				</Time>
				<Currency>
					<xsl:value-of select="td[4]"/>
				</Currency>
        <xsl:variable name="amount" select="td[5]"/>
        <Amount>
          <xsl:value-of select="csharp:Replace($amount, ' ', '')"/>
        </Amount>
        <xsl:variable name="phone" select="td[6]"/>
        <xsl:variable name="comment" select="td[7]"/>
        <xsl:variable name="fulltext" select="concat($phone, '; ', $comment)"/>
        <Other>
          <xsl:value-of select="csharp:SuperTrimAll($fulltext)"/>
        </Other>
      </MoneyOrder>
    </xsl:if>
    
		</xsl:for-each>
    </MoneyOrders>
  </xsl:template>

</xsl:stylesheet>

