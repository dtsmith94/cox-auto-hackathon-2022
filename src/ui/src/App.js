import DragAndDropFile from "./Components/DragAndDropFiles";
import NavBar from "./Components/NavBar";

import "./App.css";

function App() {
  return (
    <div className="App">
      <NavBar />
      <h2>Using AI for image recognition</h2>
      <div className="AppDescription">
        You can try to upload an image of a car and we will extract some useful information using AI. 
      </div>
      <DragAndDropFile />
    </div>
  );
}

export default App;
