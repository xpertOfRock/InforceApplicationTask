import React, { useEffect, useState } from "react";
import { deleteRecord, fetchRecords } from "../../services/shortenedUrl";
import { getCurrentUserId, isAuthorized } from "../../services/auth";
import CreateRecordForm from "../CreateRecord/CreateRecordForm";
import "./Urls.css";
import { useNavigate } from "react-router-dom";

function Urls() {
  const [urls, setUrls] = useState([]);
  const navigate = useNavigate();

  const loadRecords = () => {
    fetchRecords().then((data) => setUrls(data));
  };
  
  useEffect(() => {
    loadRecords();
  }, []);

  const handleViewClick = (id) => {
    navigate(`/urls/details/${id}`);
  };

  const handleDeleteClick = (id) => {
    deleteRecord(id).then(() => loadRecords());
  }
  return (
    <div className="urls-container">
      <h1 className="urls-title">URL Shortener</h1>
      {isAuthorized() ? (<CreateRecordForm onNewRecord={loadRecords} />) : null}
      <table className="urls-table">
        <thead>
          <tr>
            <th>Original URL</th>
            <th>Short URL</th>
            {isAuthorized() ? (
              <th>Actions</th>
            ) : null }           
          </tr>
        </thead>
        <tbody>
          {urls.map((item) => (
            <tr key={item.id}>
              <td>{item.originalUrl}</td>
              <td>{item.shortCode}</td>
              {isAuthorized() ? (
                <td>
                <div className="button-div">                   
                      <button
                        onClick={() => handleViewClick(item.id)}
                        className="redirect-button"
                      >               
                        View
                      </button>                   
                    {getCurrentUserId() === item.createdBy ? (
                      <button
                        onClick={() => handleDeleteClick(item.id)}
                        className="delete-button"
                      >
                        Delete
                      </button>
                    ) : null}
                </div>               
              </td>
              ) : null}
              
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

export default Urls;
