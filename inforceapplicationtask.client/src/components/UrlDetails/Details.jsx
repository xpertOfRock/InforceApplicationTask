import { useEffect, useState } from "react";

function Details(){
    const [details, setDetails] = useState();

    useEffect(() => {
        
    },[])

    const formatDateTime = (dateStr) => {
        const dateObj = new Date(dateStr);
        const date = dateObj.toLocaleDateString();
        const time = dateObj.toLocaleTimeString();
        return (
          <>
            <div>{date}</div>
            <div className="time-indent">{time}</div>
          </>
        );
      };
    
    return(
        <div><h1>Hello</h1></div>
    );
}

export default Details;