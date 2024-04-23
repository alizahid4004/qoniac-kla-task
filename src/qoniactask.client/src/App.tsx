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
  const [className, setClassName] = useState("border-green-500");
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
      setClassName("border-red-500");
      setValue("");
      return;
    }

    if (Number(_value) > max) {
      setErrorMessage(`Max: ${prefix}${max}`);
      setClassName("border-red-500");
      setValue(_value);
      return;
    }

    if (Number(_value) < min) {
      setErrorMessage(`Min: ${prefix}${max}`);
      setClassName("border-red-500");
      setValue(_value);
      return;
    }

    setClassName("border-green-500");
    setValue(_value);
    setValues(_values);
  };

  return (
    <div
  className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8"
>
  <div className="sm:mx-auto sm:w-full sm:max-w-sm">
    <h1
      className="mt-10 text-center text-2xl font-bold leading-9 tracking-tight text-gray-900"
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
        <div className="mt-2">
          <CurrencyInput
            id="currency-input"
            name="currency-input"
            className={`block w-full rounded-md border-0 py-2 px-2 size-12 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-base sm:leading-6 ${className}`}
            value={value}
            onValueChange={handleOnValueChange}
            placeholder="Please enter a value"
            prefix={prefix}
            step={1}
            allowNegativeValue={false}
            fixedDecimalLength={fixedDecimalLength}
            decimalSeparator={decimalSeparator}
            groupSeparator={groupSeparator}
            disableAbbreviations={true}
            maxLength={11}
          />
        </div>
      </div>

      <div>
        <button
          type="button"
          className="flex w-full justify-center size-12 rounded-md bg-indigo-600 px-3 py-1.5 text-base font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
        >
          Describe
        </button>
      </div>
    </form>
  </div>
</div>
  );
};

export default App;
