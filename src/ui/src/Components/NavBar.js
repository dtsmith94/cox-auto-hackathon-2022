import Container from "react-bootstrap/Container";
import Navbar from "react-bootstrap/Navbar";
import "./../App.css";

const NavBar = () => {
  return (
    <Navbar bg="dark" variant="dark">
      <Container>
        <Navbar.Brand href="#home">
            <img
              alt=""
              src={require('../images/cox-auto-logo.png')}
              width="auto"
              height="30"
              className="d-inline-block align-top"
            />{' '}
            Europe Hackathon '22
          </Navbar.Brand>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
      </Container>
    </Navbar>
  );
};

export default NavBar;
