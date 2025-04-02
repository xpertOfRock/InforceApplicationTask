import axios from "axios";
import { getCurrentToken } from "./auth";

const API_URL = "https://localhost:6062/api/ShortenedUrl";


export const fetchRecords = async (filter) => {
    try {       
      const result = await axios.get(API_URL);
      console.log(result.data);
      return result.data;
    } catch (e) {
      console.error(e);
    }
  };
  
  export const fetchRecord = async (id) => {  
    try {
      const response = await axios.get(`${API_URL}/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      if (response.status !== 200) {
        if (response.status === 401) {
          alert("Unauthorized: required authorization.");
        } else if (response.status === 403) {
          alert("You cannot add a new record.");
        } else {
          alert("An error occurred while adding a new record.");
        }
      }
      return response.data;
    } catch (e) {
      console.error(e);
    }
  };
  
  export const createRecord = async (inputUrl) => {
    try {
      const token = getCurrentToken();

      const response = await axios.post(API_URL, { url: inputUrl },
         {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );
      if (response.status !== 200) {
        console.log(response.status);
        if (response.status === 401) {
          alert("Unauthorized: required authorization.");
        } else if (response.status === 403) {
          alert("You cannot add a new record.");
        } else { 
          alert("Error could be caused by user trying to shorten an existing url.")
        }
      }
      return response.status;
    } catch (error) {
      console.log(error.status);   
      if (error.status !== 200) {
        console.log(response.status);
        if (error.status === 401) {
          alert("Unauthorized: required authorization.");
        } else if (error.status === 403) {
          alert("You cannot add a new record.");
        } else { 
          alert("Error could be caused by user trying to shorten an existing url.")
        }
      }   
    }
  };
  
  export const updateRecord = async (id, originalUrl) => {
    try {
      const token = getCurrentToken();
      const response = await axios.put(
        `${API_URL}/${id}`,
        { originalUrl },
        {
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        }
      );
      if (response.status !== 200) {
        if (response.status === 401) {
          alert("Unauthorized: required authorization.");
        } else if (response.status === 403) {
          alert("You cannot update the record.");
        } else {
          alert("An error occurred while updating the record.");
        }
      }
      return response.status;
    } catch (error) {
      console.log(error);  
    }
  };
  
  export const deleteRecord = async (id) => {
    try {
      const token = getCurrentToken();
      const response = await axios.delete(`${API_URL}/${id}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      if (response.status !== 200) {
        if (response.status === 401) {
          alert("Unauthorized: required authorization.");
        } else if (response.status === 403) {
          alert("You cannot delete the record.");
        } else {
          alert("An error occurred while deleting the record.");
        }
      }
      return response.status;
    } catch (error) {
      console.log(error);  
    }
  };