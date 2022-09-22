import DragAndDropFile from "./Components/DragAndDropFiles";
import NavBar from "./Components/NavBar";

import "./App.css";

function App() {
  return (
    <div className="App">
      <NavBar />
      <h2>Using AI for image recognition</h2>
      <div className="AppDescription">
        You can upload an image and we will try to extract some useful
        information. Could be useful to extract a VRM from an image, or to
        identify other contents of an image.
      </div>
      <DragAndDropFile />
    </div>
  );
}

export default App;
