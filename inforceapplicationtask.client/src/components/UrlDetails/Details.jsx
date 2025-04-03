import { useEffect, useState } from "react";
import { useParams , useNavigate } from "react-router-dom";
import { fetchRecord } from "../../services/shortenedUrl";
import { getCurrentUserId } from "../../services/auth";

import './Details.css';


function Details(){
  const { id } = useParams();
  const navigate = useNavigate();
  const [details, setDetails] = useState(null);

  const getShortUrlRedirect = (code) => {
      window.location.href = `https://localhost:6062/redirect/${code}`;
  }

  const formatDateTime = (dateStr) => {
        const dateObj = new Date(dateStr);
        const date = dateObj.toLocaleDateString();
        const time = dateObj.toLocaleTimeString();
        return `${date} ${time}`;
      };

  useEffect(() => {
    async function fetchDetails() {
      const data = await fetchRecord(id).then((data) => setDetails(data));
    }
    fetchDetails();
  }, [id]);

    if (!details) {
    return <div className="details-container">Loading...</div>;
  }

  return (
    <div className="details-container">
      <h1 className="details-title">Short Url Info</h1>
      <div className="details-card">
        <p>
          <span className="details-label">Original URL:</span> {details.originalUrl}
        </p>
        <p>
          <span className="details-label">Short URL:</span> {details.shortCode}
        </p>
        <p>
          <span className="details-label">Created Date:</span> {formatDateTime(details.createdDate)}
        </p>
        <p>
          <span className="details-label">Created By:</span> {details.createdBy} {getCurrentUserId() === details.createdBy ? (<small>(You)</small>) : null}
        </p>
      </div>
      <div className="details-actions">
        <button
          className="redirect-button"
          onClick={() => getShortUrlRedirect(details.shortCode)}
        >
          Redirect
        </button>
        <button className="back-button" onClick={() => navigate(-1)}>
          Back
        </button>
      </div>
    </div>
  );
};

export default Details;