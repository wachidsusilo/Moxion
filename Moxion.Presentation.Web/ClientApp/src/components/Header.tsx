import logo from "../assets/svg/logo.svg"

interface IHeaderProps {
    className?: string
}

function Header({className}: IHeaderProps) {
    return (
        <div className={`flex items-center w-full h-[92px] shadow-[0px_1px_0px_0px_rgba(255_255_255_/_0.1)] ${className}`}>
            <div className="ml-8 flex items-center gap-4">
                <img className="w-[40px] h-[40px]" src={logo} alt="logo" />
                <span className="font-normal text-3xl">Moxion</span>
            </div>
        </div>
    )
}

export default Header