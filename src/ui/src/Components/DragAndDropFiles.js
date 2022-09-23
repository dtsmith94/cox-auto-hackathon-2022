import { useEffect, useState } from "react";
import { FileUploader } from "react-drag-drop-files";
import axios from "axios";
import { Button, Card, Row, Col, Container } from "react-bootstrap";
import { BallTriangle } from "react-loader-spinner";

const fileTypes = ["JPEG", "PNG", "JPG"];

const DragAndDropFile = () => {
  const [file, setFile] = useState(null);
  const [isLoading, setIsLoading] = useState(false);
  const [result, setResult] = useState(null);
  const [error, setError] = useState(null);

  const handleChange = (file) => {
    setResult(null);
    setFile(file);
  };

  useEffect(() => {
    if (file && file[0]) {
      importFile();
    }
  }, [file]);

  const importFile = async (e) => {
    const formData = new FormData();
    formData.append("file", file[0]);
    setError(null);
    setIsLoading(true);
    try {
      const res = await axios.post(
        //"https://functionappcoxhackathon2022.azurewebsites.net/api/ReadFileAsStream",
        "http://localhost:7071/api/ReadFileAsStream",
        formData
      );
      setResult(res.data);
    } catch (ex) {
      console.log("ex", ex);
      setError("Error when requesting the endpoint.", ex.message);
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
      <p>{file && file[0] ? "" : "no files uploaded yet"}</p>
      {file && file[0] && (
        <div style={{ maxWidth: "300px" }}>
          <img
            src={URL.createObjectURL(file[0])}
            style={styles.image}
            alt="Thumb"
          />
        </div>
      )}
      {isLoading && (
        <BallTriangle
          height={100}
          width={100}
          radius={5}
          color="#00528a"
          ariaLabel="ball-triangle-loading"
          wrapperClass={{}}
          wrapperStyle=""
          visible={true}
        />
      )}
      {error ? error : ''}
      {file && !isLoading && result && !error && (
        <>
          <img
            alt=""
            src={require("../images/arrow-down-sign-to-navigate.png")}
            width="auto"
            //height="30"
            className="d-inline-block align-top"
          />
          <Card
            className="text-center"
            style={{ width: "1000px", margin: "15px" }}
          >
            <Card.Header>
              <Container>
                <Row>
                  <Col>
                    {result.isVehicle
                      ? "✔️ It's a vehicle!"
                      : " ❌ It's not a vehicle."}
                  </Col>
                  <Col>
                    Description: <br />
                    <b>
                      {result.description
                        ? result.description
                        : "No description available."}
                    </b>
                  </Col>
                  <Col>
                    Unit Type: <b> {result.unitType} </b>
                  </Col>
                </Row>
              </Container>
            </Card.Header>
            <Card.Body>
              <Container>
                <Row>
                  <Col>
                    VRM: <br />
                    <b>{result.vrm ? result.vrm : "No VRM detected."}</b>
                  </Col>
                  <Col>
                    Brand: <br />
                    <b>{result.brand ? result.brand : "No Brand detected."}</b>
                  </Col>
                  <Col>
                    Image Proportion Of Car: <br />{" "}
                    <b>
                      {result.imageProportionOfCar
                        ? result.imageProportionOfCar + "%"
                        : "No propotion detected."}
                    </b>
                  </Col>
                </Row>
                <ColoredLine color="dark-grey" />
                <Row>
                  <Col>
                    Thumbnail with Smart Cropping: <br />
                    <img
                      src={"data:image/png;base64," + result.imageThumbnail}
                      style={styles.smartCroppingImage}
                      alt="Thumb"
                    />
                  </Col>
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
  smartCroppingImage: {
    maxWidth: "150px",
    maxHeight: 320,
    paddingBottom: "15px",
  },
};

const ColoredLine = ({ color }) => (
  <hr
    style={{
      color: color,
      backgroundColor: color,
      height: 1,
    }}
  />
);
