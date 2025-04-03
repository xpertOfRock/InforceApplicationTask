import axios from 'axios';
import { getCurrentToken, getCurrentUserId } from './auth';

const API_URL = "https://localhost:6062/api/About";

export const fetchAbout = async () => {
    try{
        const response = await axios.get(`${API_URL}`);
        return response.data;
    }catch(error){
        alert("An error occured while fetching data.")
    }
};

export const updateAbout = async (description) => {

    const token = getCurrentToken();
    
    try{
        const response = await axios.put(`${API_URL}`, 
            JSON.stringify(description),
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
    }catch(error){
        console.log(error);
        alert("An error occured while updating about page.");
    }
};