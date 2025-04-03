import React, { useState, useEffect } from "react";
import { fetchAbout, updateAbout } from "../../services/about";
import { isAdmin } from "../../services/auth";
import "./About.css";

function About() {
  const [about, setAbout] = useState("");
  const [editMode, setEditMode] = useState(false);
  const [newDescription, setNewDescription] = useState("Write new description here...");

  const fetchDescription = async () => {
    const data = await fetchAbout();
    setAbout(data);
    setNewDescription(data.description);
  }

  useEffect(() => {   
    fetchDescription();
  }, []);

  const handleSave = async () => {
    await updateAbout(newDescription);
    fetchDescription();
    setEditMode(false);
  };

  return (
    <div className="about-container">
      <h1 className="about-title">About URL Shortener</h1>
      
      {editMode ? (
        <div className="edit-area">
          <textarea
            className="about-textarea"
            value={newDescription}
            onChange={(e) => setNewDescription(e.target.value)}
          />
          <div className="button-group">
            <button className="save-button" onClick={handleSave}>
              Save
            </button>
            <button className="cancel-button" onClick={() => { setNewDescription(about.description); setEditMode(false); }}>
              Cancel
            </button>
          </div>
        </div>
      ) : (
        <textarea
            className="about-textarea"
            value={about.description}
          />
      )}
      {isAdmin() && !editMode && (
        <button className="edit-button" onClick={() => setEditMode(true)}>
          Edit
        </button>
      )}
    </div>
  );
}

export default About;