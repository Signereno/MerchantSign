<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="2.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:fn="http://www.w3.org/2005/02/xpath-functions" xmlns:xdt="http://www.w3.org/2005/02/xpath-datatypes">
  <xsl:template match="/">   
    <HTML>      
      <BODY>        
        <xsl:for-each select="Person">          
          <table>
            <tr>
              <td>
                <i>
                  <xsl:value-of select="Name"/><br/>
                  <xsl:value-of select="Street"/><br/>
                  <xsl:value-of select="Zip"/> <xsl:value-of select="City"/>
                </i>
              </td>
            </tr>
            <tr>
              <td align="right"> 24. desember, Oslo</td>
            </tr>
            <tr>
              <td>
                <h2>Eksempel på XML/XSL</h2>
              </td>
            </tr>
            <tr>
              <td>
                XML transformert med XSL er veldig godt egnet til å generere dokumenter 
                 basert på data, f.eks. fra en database. <p/> 
                Med vennelig hilsen<br/>
                Kari Nordmann
              </td>
            </tr>
          </table>          
        </xsl:for-each>        
      </BODY>      
    </HTML>    
  </xsl:template>  
</xsl:stylesheet>