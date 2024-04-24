import { useEffect, useState } from "react";
import CurrencyInput, {
  CurrencyInputProps,
  CurrencyInputOnChangeValues,
} from "react-currency-input-field";
import "./App.css";

const App = () => {
  const max = 999999999.99;
  const min = 0;
  const prefix = "$";
  const decimalSeparator = ",";
  const groupSeparator = " ";
  const fixedDecimalLength = 2;

  const [errorMessage, setErrorMessage] = useState("");
  const [borderClassName, setClassName] = useState("ring-gray-300");
  const [value, setValue] = useState<string | number>(123.45);
  const [values, setValues] = useState<CurrencyInputOnChangeValues>();

  const handleOnValueChange: CurrencyInputProps["onValueChange"] = (
    _value,
    name,
    _values,
  ) => {
    console.log(_value);
    console.log(_values);

    if (!_value) {
      setClassName("ring-red-500");
      setValue("");
      return;
    }

    if (Number(_value) > max) {
      setErrorMessage(`Max: ${prefix}${max}`);
      setClassName("ring-red-500");
      setValue(_value);
      return;
    }

    if (Number(_value) < min) {
      setErrorMessage(`Min: ${prefix}${max}`);
      setClassName("ring-red-500");
      setValue(_value);
      return;
    }

    setClassName("ring-green-500");
    setValue(_value);
    setValues(_values);
  };

  return (
    <div
  className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8"
>
  <div className="sm:mx-auto sm:w-full sm:max-w-sm">
    <h1
      className="mt-10 text-center text-3xl font-bold leading-9 tracking-tight text-gray-900"
    >
      Currency Describer
    </h1>
  </div>

  <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
    <form className="space-y-6">
      <div>
        <div className="flex items-center justify-between">
          <label
            htmlFor="currency-input"
            className="block font-medium leading-6 text-gray-900"
          >
            Enter value to be described
          </label>
        </div>
        <div className="mt-2 relative">
          <CurrencyInput
            id="currency-input"
            name="currency-input"
            className={`block w-full rounded-md border-0 px-6 size-14 text-gray-900 shadow-sm ring-2 ring-inset ${borderClassName} placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-blue-600 sm:text-lg sm:leading-6`}
            value={value}
            onValueChange={handleOnValueChange}
            placeholder="Please enter a value"
            step={1}
            allowNegativeValue={false}
            fixedDecimalLength={fixedDecimalLength}
            decimalSeparator={decimalSeparator}
            groupSeparator={groupSeparator}
            disableAbbreviations={true}
            maxLength={11}
          />
          <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
            <span className="text-lg font-bold">
              {prefix} 
            </span> 
          </div> 
        </div>
      </div>

      <div>
        <button
          type="button"
          className="flex w-full justify-center size-12 rounded-md bg-blue-600 px-3 py-3 text-lg font-semibold leading-6 text-white shadow-sm hover:bg-blue-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600"
        >Describe
        </button>
      </div>
    </form>
  </div>
</div>
  );
};

export default App;
