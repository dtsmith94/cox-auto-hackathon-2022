import DragAndDropFile from "./Components/DragAndDropFiles";
import NavBar from "./Components/NavBar";

import "./App.css";

function App() {
  return (
    <div className="App">
      <NavBar />
      <h2>AI Photo Booth</h2>
      <div className="AppDescription">
        You can try to upload an image of a car and we will extract some useful information!
      </div>
      <DragAndDropFile />
    </div>
  );
}

export default App;
