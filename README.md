# MDClassDiagram

This code was written as part of Google Summer of Code, 2015. <br>
Org - Mono

MDClassDiagram is an addin to monodevelop/xamarain studio that helps developers view and generate class diagrams for their codes. Monohotdraw library has been made use of for graphic purposes.<br><br>
The code has 3 main folders:<br>
<b>1. Backend</b><br>
This folder contains backend code that fetches code details. It makes use of the Roslyn language service (NRefactory6). This has been used keeping in mind the future release of monodevelop.<br><br>
<b>2. Figures</b><br>
This folder contains code that extends the monohotdraw library to suit the needs of class diagrams.<br><br>
<b>3. LayoutAlgorithms</b><br>
This folder contains code that contains 2 layout algorithms at the moment. One is the brute layout algorithm and the other is thre Treelayout algorithm which is visually better.<br><br>
<br>
Currently, the class diagram has the following features:<br>
1. Easy positioning and adjusting of class diagrams.<br>
2. Expand and collapse features for clutter free viewing.<br>
3. Quick code view from class diagram.<br>
4. Easy modifications on Inheritance links.<br>
5. Icons<br><br>

