import React from 'react';
import './AppConfig.css';
import {MatchConfig} from "./models/match-config";
import {useState,useEffect} from 'react';
import axios from 'axios';

const AppConfig: React.FC<{}> = props => {

   const [configs,setConfigs]=useState<MatchConfig[]>([])

    const loadData = async () => {
        try {
            let url = './api/config/match';
            let res = await axios.get(url);
            let data = res.data;
            setConfigs(data);
        } catch (e) {
            console.error(e);
        }
    };

   useEffect(()=>{
       loadData();
   },[])


    return (

        <ul> {configs?(
            <li>{configs.length}</li>
        ):<div/>}
            <li>Configs</li>
        </ul>


    );
}

export default AppConfig;
