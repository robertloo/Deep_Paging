# Deep_Paging

This program runs a Solr URL query, reads the document to find the 'nextCurserMark' attribute, uses that to create the next query, and so on until there are no more documents to retrieve.

See page for examples on consturctin the base URL. The 'cursorMark' parameter is not needed. This is added by the program.
http://heliosearch.org/solr/paging-and-deep-paging/