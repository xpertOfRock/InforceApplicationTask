import React, { useState } from "react";
import { createRecord } from "../../services/shortenedUrl";
import "./CreateRecordForm.css";

const CreateRecordForm = ({ onNewRecord }) => {
  const [url, setUrl] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    try {
      const status = await createRecord(url);
      if (status === 200) {
        onNewRecord();
        setUrl("");
      }
    } catch (err) {       
        setError("Error! Try again!");
        alert("Error could be caused by user trying to shorten an existing url.");
    }
  };

  return (
    <div className="create-record-form">
      <h2 className="form-title">Create New Short URL</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="text"
          placeholder="Enter URL"
          value={url}
          onChange={(e) => setUrl(e.target.value)}
          className="form-input"
        />
        {error && <p className="form-error">{error}</p>}
        <button type="submit" className="form-button">Shorten!</button>
      </form>
    </div>
  );
};

export default CreateRecordForm;