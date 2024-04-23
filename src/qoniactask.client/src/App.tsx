import { useEffect, useState } from "react";
import "./App.css";

interface Forecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}

function App() {

  (<div>
    
  <h1 className="text-3xl font-bold underline">Currency Describer</h1>
  <CurrencyInput
  id="validationCustom01"
  name="input-1"
  className={`form-control ${className}`}
  value={value}
  onValueChange={handleOnValueChange}
  placeholder="Please enter a number"
  prefix={prefix}
  step={1}
  />
  </div>

)
  
}

export default App;
