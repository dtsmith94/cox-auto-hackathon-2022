import { useState } from "react";
import { FileUploader } from "react-drag-drop-files";
import axios from "axios";
import { Button, Card, Row, Col, Container } from "react-bootstrap";
import resultSuccess from "./result";

const fileTypes = ["JPEG", "PNG", "JPG"];

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
        "https://localhost:44323/api/importfile",
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
      <p>
        {file && file[0].name
          ? `File name: ${file[0].name}`
          : "no files uploaded yet"}
      </p>
      {file && (
        <img
          src={URL.createObjectURL(file[0])}
          style={styles.image}
          alt="Thumb"
        />
      )}
      {file && !isLoading && !result && (
        <Button variant="primary" onClick={importFile}>
          Let's try the AI
        </Button>
      )}
      {isLoading && "Loading..."}

      {file && !isLoading && result && (
        <>
          <Card
            className="text-center"
            style={{ width: "1000px", margin: "15px" }}
          >
            <Card.Header>Description: {resultSuccess.description}</Card.Header>
            <Card.Body>
              <Container>
                <Row>
                  <Col></Col>
                </Row>
                <Row>
                  <Col>{resultSuccess.VRM}</Col>
                  <Col>{resultSuccess.Brand}</Col>
                  <Col>
                    Image proportion of car: <br />{" "}
                    {resultSuccess.ImageProportion}%
                  </Col>
                  <Col>Image orientation: {resultSuccess.ImageOrientation}</Col>
                </Row>
              </Container>
            </Card.Body>
          </Card>

          <Button variant="primary" onClick={importFile}>
            Try again
          </Button>
        </>
      )}
    </div>
  );
};

export default DragAndDropFile;

const styles = {
  image: { maxWidth: "100%", maxHeight: 320, paddingBottom: "15px" },
};
