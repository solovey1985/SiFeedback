import React from "react";
import ReactDOM from "react-dom";
import { createBrowserHistory } from "history";
import { Router, Route, Switch } from "react-router-dom";

import "bootstrap/dist/css/bootstrap.css";
import "./assets/scss/now-ui-dashboard.css";
import "./assets/css/demo.css";

import indexRoutes from "routes/index.jsx";
import { Card, CardBody, CardTitle, CardSubtitle, CardText, CardLink } from 'reactstrap';
const hist = createBrowserHistory();
ReactDOM.render(
  <Card style={{width: '20rem'}}>
  <CardBody>
      <CardTitle>Card title</CardTitle>
      <CardSubtitle className="mb-2 text-muted">Card subtitle</CardSubtitle>
      <CardText>Some quick example text to build on the card title and make up the bulk of the card's content.</CardText>
      <CardLink href="/#/">Card link</CardLink>
      <CardLink href="/#/">Another link</CardLink>
  </CardBody>
</Card>,
   document.getElementById("root")
);
