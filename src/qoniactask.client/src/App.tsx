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

  const [message, setMessage] = useState<string>("");
  const [messageClass, setMessageClass] = useState<string>("text-gray-900");
  const [borderClassName, setBorderClassName] = useState<string>("ring-gray-300");
  const [validInput, setValidInput] = useState<boolean>(true);
  const [value, setValue] = useState<string | number>(123.45);
  const [values, setValues] = useState<CurrencyInputOnChangeValues>();

  const handleOnValueChange: CurrencyInputProps["onValueChange"] = (
    _value,
    name,
    _values,
  ) => {
    if (!_value) {
      setBorderClassName("ring-red-500");
      setValidInput(false);
      setValue("");
      return;
    }

    if (Number(_value) > max) {
      setMessage(`Max: ${prefix}${max}`);
      setMessageClass("text-red-500");
      setBorderClassName("ring-red-500");
      setValidInput(false);
      setValue(_value);
      return;
    }

    if (Number(_value) < min) {
      setMessage(`Min: ${prefix}${max}`);
      setMessageClass("text-red-500");
      setBorderClassName("ring-red-500");
      setValidInput(false);
      setValue(_value);
      return;
    }

    setBorderClassName("ring-green-500");
    setMessageClass("text-gray-900");
    setMessage("")
    setValidInput(true);
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
            decimalsLimit={fixedDecimalLength}
            decimalSeparator={decimalSeparator}
            groupSeparator={groupSeparator}
            disableAbbreviations={true}
            maxLength={12}
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
          className={`${validInput ? '' : 'opacity-50'} flex w-full justify-center size-12 rounded-md bg-blue-600 px-3 py-3 text-lg font-semibold leading-6 text-white shadow-sm hover:bg-blue-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600`}
          disabled={!validInput}
        >Describe
        </button>
      </div>
    </form>
  </div>

  <div className="sm:mx-auto sm:w-full sm:max-w-sm">
    <p
      className={`mt-10 text-center text-xl font-bold leading-9 tracking-tight ${messageClass}`}
    >
      {message}
    </p>
  </div>
</div>
  );
};

export default App;
