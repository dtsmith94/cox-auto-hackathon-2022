import { useState } from "react";
import { FileUploader } from "react-drag-drop-files";
import axios from "axios";
import { Button } from "react-bootstrap";

const fileTypes = ["JPEG", "PNG"];

const DragAndDropFile = () => {
  const [file, setFile] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [result, setResult] = useState(null);
  const handleChange = (file) => {
    setFile(file);
  };

  const importFile = async (e) => {
    const formData = new FormData();
    formData.append("file", file[0]);
    setIsLoading(true);
    try {
      const res = await axios.post(
        "http://localhost:7071/api/ReadFileAsStream",
        formData
      );
      setResult(res);
    } catch (ex) {
      console.log("ex", ex);
      setResult("Error when requesting the endpoint.", ex.message);
    }
    setIsLoading(false);
  };

  return (
    <div className="DragAndDrop">
      <FileUploader
        multiple={true}
        handleChange={handleChange}
        name="file"
        types={fileTypes}
      />
      <p>{file ? `File name: ${file[0].name}` : "no files uploaded yet"}</p>
      {(file && !isLoading) && (
        <Button variant="primary" onClick={importFile}>
          Upload
        </Button>
      )}
      {isLoading && ('Loading...')}
      {(result && !isLoading) && result}
    </div>
  );
};

export default DragAndDropFile;